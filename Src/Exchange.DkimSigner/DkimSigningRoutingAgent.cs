using ConfigurationSettings;
using Microsoft.Exchange.Data.Transport;
using Microsoft.Exchange.Data.Transport.Routing;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

namespace Exchange.DkimSigner
{
    /// <summary>
    /// Signs outgoing MIME messages according to the DKIM protocol.
    /// </summary>
    public sealed class DkimSigningRoutingAgent : RoutingAgent
    {
        /// <summary>
        /// The object that knows how to sign messages.
        /// </summary>
        private DkimSigner dkimSigner;

        /// <summary>
        /// Watcher for changes on the settings file causing a reload of the settings when changed
        /// </summary>
        private FileSystemWatcher watcher;

        /// <summary>  
        /// This context to allow Exchange to continue processing a message  
        /// </summary>  
        private AgentAsyncContext agentAsyncContext; 

        /// <summary>
        /// Initializes a new instance of the <see cref="DkimSigningRoutingAgent"/> class.
        /// </summary>
        /// <param name="dkimSigner">The object that knows how to sign messages.</param>
        public DkimSigningRoutingAgent()
        {
            Logger.LogDebug("Initializing DkimSigner");
            Settings config = new Settings();
            config.InitHeadersToSign();

            this.dkimSigner = new DkimSigner();

            this.LoadSettings();
            this.WatchSettings();

            this.OnCategorizedMessage += this.WhenMessageCategorized;
            Logger.LogDebug("DkimSigner initiallized");
        }

        private void LoadSettings()
        {
            Settings config = new Settings();
            config.InitHeadersToSign();

            if(config.Load(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "settings.xml")))
            {
                this.dkimSigner.UpdateSettings(config);
                Logger.logLevel = config.Loglevel;
                Logger.LogInformation("Exchange DKIM settings loaded: " + config.SigningAlgorithm.ToString() + ", Canonicalization Header Algorithm: " + config.HeaderCanonicalization.ToString() + ", Canonicalization Body Algorithm: " + config.BodyCanonicalization.ToString() + ", Number of domains: " + this.dkimSigner.GetDomains().Count);
            }
            else
            {
                Logger.LogError("Couldn't load the settings file.\n");
            }
        }

        public void WatchSettings()
        {
            string filename = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "settings.xml");

            // Create a new FileSystemWatcher and set its properties.
            watcher = new FileSystemWatcher();
            watcher.Path = Path.GetDirectoryName(filename);

            // Watch for changes in LastAccess and LastWrite times, and the renaming of files or directories.
            watcher.NotifyFilter = NotifyFilters.LastWrite;

            // Only watch text files.
            watcher.Filter = Path.GetFileName(filename);

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(this.OnChanged);
            watcher.Created += new FileSystemEventHandler(this.OnChanged);

            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }

        // Define the event handlers.
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            Logger.LogInformation("Detected settings file change. Reloading...");
            this.LoadSettings();
        }

        /// <summary>
        /// Fired when Exchange has performed content conversion, if it was required.
        /// The OnCategorizedMessage event is the last event that fires before the server
        /// puts the message in the delivery queue. This means it's a good time to sign the
        /// message, because it's unlikely that anything else will diddle with the message
        /// and invalidate our signature. (Our transport agent will need to be the last to run,
        /// though.)
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="Microsoft.Exchange.Data.Transport.Routing.QueuedMessageEventArgs"/> instance containing the event data.</param>
        [SuppressMessage("Microsoft.Design", "CA1031", Justification = "If an exception is thrown, then the message is eaten by Exchange. Better to catch the exception and write it to a log than to end up with a non-functioning MTA.")]
        private void WhenMessageCategorized(CategorizedMessageEventSource source, QueuedMessageEventArgs e)
        {
            Logger.LogDebug("Got new message, checking if I can sign it...");
            try
            {
                this.agentAsyncContext = this.GetAgentAsyncContext();

                this.SignMailItem(e.MailItem);
            }
            catch (Exception ex)
            {
                Logger.LogError("Signing a mail item according to DKIM failed with an exception. Check the logged exception for details.\n" + ex.ToString());
            }
            finally
            {
                this.agentAsyncContext.Complete();
            }
        }

        /// <summary>
        /// Signs the given mail item, if possible, according to the DKIM standard.
        /// </summary>
        /// <param name="mailItem">The mail item that is to be signed, if possible.</param>
        private void SignMailItem(MailItem mailItem)
        {
            // If the mail item is a "system message" then it will be read-only here,
            // and we can't sign it. Additionally, if the message has a "TnefPart",
            // then it is in a proprietary format used by Outlook and Exchange Server,
            // which means we shouldn't bother signing it.
            if (!mailItem.Message.IsSystemMessage && mailItem.Message.TnefPart == null)
            {
                /* Check if we have a valid From address */
                if (!mailItem.FromAddress.IsValid || mailItem.FromAddress.DomainPart == null)
                {
                    Logger.LogWarning("Invalid from address: '" + mailItem.FromAddress + "'. Not signing email.");
                    return;
                }

                /* If domain was found in define domain configuration */
                if (this.dkimSigner.GetDomains().ContainsKey(mailItem.FromAddress.DomainPart))
                {
                    DomainElement domain = this.dkimSigner.GetDomains()[mailItem.FromAddress.DomainPart];

                    using (Stream stream = mailItem.GetMimeReadStream())
                    {
                        Logger.LogDebug("Domain found: '"+domain.Domain+"'. I'll sign the message.");
                        string dkim = this.dkimSigner.CanSign(domain, stream);

                        if (dkim.Length != 0)
                        {
                            Logger.LogInformation("Signing mail with header: " + dkim);

                            stream.Seek(0, SeekOrigin.Begin);
                            byte[] inputBuffer = ReadFully(stream);
                            stream.Close();

                            using (Stream outputStream = mailItem.GetMimeWriteStream())
                            {
                                try
                                {
                                    this.dkimSigner.Sign(inputBuffer, outputStream, dkim);
                                }
                                catch (Exception ex)
                                {
                                    Logger.LogError("Signing went terribly wrong: " + ex.ToString());
                                }

                                outputStream.Close();
                            }
                        } else {
                            Logger.LogDebug("Got empty signing header. Something went wrong...");
                        }

                    }
                } else {
                    Logger.LogDebug("No entry found in config for domain '" + mailItem.FromAddress.DomainPart + "'");
                }
            }
            else
                Logger.LogDebug("Message is a System message or of TNEF format. Not signing.");
        }

        /// <summary>
        /// Slurp the stream and convert it to a binary array.
        /// http://stackoverflow.com/a/221941/869402
        /// </summary>
        /// <param name="input">The stream to read</param>
        /// <returns>All the data from the stream as byte array</returns>
        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                
                return ms.ToArray();
            }
        }
    }
}