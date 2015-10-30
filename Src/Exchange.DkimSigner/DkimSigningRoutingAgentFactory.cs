using ConfigurationSettings;
using Microsoft.Exchange.Data.Transport;
using Microsoft.Exchange.Data.Transport.Routing;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace Exchange.DkimSigner
{
    /// <summary>
    /// Creates new instances of the DkimSigningRoutingAgent.
    /// </summary>
    public sealed class DkimSigningRoutingAgentFactory : RoutingAgentFactory
    {

        /// <summary>
        /// The main DKIM Signer instance used for all emails
        /// </summary>
        private DkimSigner dkimSigner;

        /// <summary>
        /// Watcher for changes on the settings file causing a reload of the settings when changed
        /// </summary>
        private FileSystemWatcher watcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="DkimSigningRoutingAgentFactory"/> class.
        /// </summary>
        public DkimSigningRoutingAgentFactory()
        {
            Logger.LogDebug("Initializing DkimSigner Service");

            Settings config = new Settings();
            config.InitHeadersToSign();

            this.dkimSigner = new DkimSigner();

            this.LoadSettings();
            this.WatchSettings();
        }

        /// <summary>
        /// When overridden in a derived class, the 
        /// <see cref="M:Microsoft.Exchange.Data.Transport.Routing.RoutingAgentFactory.CreateAgent(Microsoft.Exchange.Data.Transport.SmtpServer)"/> 
        /// method returns an instance of a routing agent.
        /// </summary>
        /// <param name="server">The server on which the routing agent will operate.</param>
        /// <returns>The <see cref="DkimSigningRoutingAgent"/> instance.</returns>
        public override RoutingAgent CreateAgent(SmtpServer server)
        {
            return new DkimSigningRoutingAgent(this.dkimSigner);
        }

        /// <summary>
        /// Load the settings from the settings.xml file and apply the new settings to the dkimSigner instance.
        /// </summary>
        private void LoadSettings()
        {
            Settings config = new Settings();
            config.InitHeadersToSign();

            if (config.Load(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "settings.xml")))
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

        /// <summary>
        /// Watch for changes in the settings.xml file and reload them if a change is detected
        /// </summary>
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

        /// <summary>
        /// Event handler for handling change detection of the file watcher for settings.xml.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            Logger.LogInformation("Detected settings file change. Reloading...");
            this.LoadSettings();
        }
    }
}