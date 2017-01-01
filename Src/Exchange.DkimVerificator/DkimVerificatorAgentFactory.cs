using Microsoft.Exchange.Data.Transport;
using Exchange.Dkim;
using Microsoft.Exchange.Data.Transport.Smtp;

namespace Exchange.DkimVerificator
{
    /// <summary>
    /// Agent factory for the DKIM Verificator
    /// </summary>
    public class DkimVerificatorAgentFactory : SmtpReceiveAgentFactory
    {

        /// <summary>
        /// The main DKIM Signer instance used for all emails
        /// </summary>
        private DkimVerificator dkimVerificator;

        /// <summary>
        /// Checks if the settings file changed during runtime.
        /// </summary>
        private SettingsWatcher settingsWatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="DkimVerificatorAgentFactory"/> class.
        /// </summary>
        public DkimVerificatorAgentFactory()
        {
            Logger.LogDebug("Initializing DkimVerificator Service");
            
            dkimVerificator = new DkimVerificator();
            settingsWatcher = new SettingsWatcher(dkimVerificator);

            settingsWatcher.LoadSettings();
            settingsWatcher.WatchSettings();
        }

        /// <summary>
        /// Creates a DkimVerificatorAgent instance.
        /// </summary>
        /// <param name="server">The SMTP server.</param>
        /// <returns>An agent instance.</returns>
        public override SmtpReceiveAgent CreateAgent(SmtpServer server)
        {
            return new DkimVerificatorAgent(dkimVerificator);
        }

    }
}
