using Microsoft.Exchange.Data.Transport;
using Microsoft.Exchange.Data.Transport.Routing;
using Exchange.Dkim;

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
        /// Checks if the settings file changed during runtime.
        /// </summary>
        private SettingsWatcher settingsWatcher;


        /// <summary>
        /// Initializes a new instance of the <see cref="DkimSigningRoutingAgentFactory"/> class.
        /// </summary>
        public DkimSigningRoutingAgentFactory()
        {
            Logger.LogDebug("Initializing DkimSigner Service");

            dkimSigner = new DkimSigner();
            settingsWatcher = new SettingsWatcher(dkimSigner);

            settingsWatcher.LoadSettings();
            settingsWatcher.WatchSettings();
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
            return new DkimSigningRoutingAgent(dkimSigner);
        }

    }
}