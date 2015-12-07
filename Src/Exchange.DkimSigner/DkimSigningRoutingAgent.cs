﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Mail;
using ConfigurationSettings;
using Microsoft.Exchange.Data.Transport;
using Microsoft.Exchange.Data.Transport.Routing;

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
        /// This context to allow Exchange to continue processing a message  
        /// </summary>  
        private AgentAsyncContext agentAsyncContext; 

        /// <summary>
        /// Initializes a new instance of the <see cref="DkimSigningRoutingAgent"/> class.
        /// </summary>
        /// <param name="dkimSigner">The object that knows how to sign messages.</param>
        /// 
        public DkimSigningRoutingAgent(DkimSigner dkimSigner)
        {
            this.dkimSigner = dkimSigner;

            OnCategorizedMessage += WhenMessageCategorized;
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
                agentAsyncContext = GetAgentAsyncContext();
#if !EX_2007_SP3 //not supported in Exchange 2007
                agentAsyncContext.Resume();
#endif
                SignMailItem(e.MailItem);
            }
            catch (Exception ex)
            {
                Logger.LogError("Signing a mail item according to DKIM failed with an exception. Check the logged exception for details.\n" + ex);
            }
            finally
            {
                agentAsyncContext.Complete();
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
                string domainPart = null;
                
                /* Check if we have a valid From address */
                if (!mailItem.FromAddress.IsValid || mailItem.FromAddress.DomainPart == null)
                {
                    // The FromAddress is empty. Try to get the domain from somewhere else (see https://github.com/Pro/dkim-exchange/issues/99)
                    string smtpAddress = (mailItem.Message != null && mailItem.Message.Sender != null) ? mailItem.Message.Sender.SmtpAddress : null;
                    if (smtpAddress != null && smtpAddress.Length > 0)
                    {
                        try
                        {
                            domainPart = new MailAddress(smtpAddress).Host;
                        }
                        catch (FormatException)
                        {
                            // do nothing
                        }
                    }
                    if (domainPart == null)
                    {
                        Logger.LogWarning("Invalid from address '" + mailItem.FromAddress + "' and invalid SmtpAddress '" + smtpAddress + "'. Not signing email.");
                        return;
                    }
                }
                else
                {
                    // from address is valid
                    domainPart = mailItem.FromAddress.DomainPart;
                }

                /* If domain was found in define domain configuration */
                if (dkimSigner.GetDomains().ContainsKey(domainPart))
                {
                    DomainElement domain = dkimSigner.GetDomains()[domainPart];

                    using (Stream stream = mailItem.GetMimeReadStream())
                    {
                        Logger.LogDebug("Domain found: '"+domain.Domain+"'. I'll sign the message.");
                        string dkim = dkimSigner.CanSign(domain, stream);

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
                                    dkimSigner.Sign(inputBuffer, outputStream, dkim);
                                }
                                catch (Exception ex)
                                {
                                    Logger.LogError("Signing went terribly wrong: " + ex);
                                }

                                outputStream.Close();
                            }
                        } else {
                            Logger.LogDebug("Got empty signing header. Something went wrong...");
                        }

                    }
                } else {
                    Logger.LogDebug("No entry found in config for domain '" + domainPart + "'");
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
