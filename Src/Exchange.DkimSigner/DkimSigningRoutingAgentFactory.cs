namespace Skiviez.Wolverine.Exchange.DkimSigner
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Reflection;
    using log4net;
    using log4net.Config;
    using Microsoft.Exchange.Data.Transport;
    using Microsoft.Exchange.Data.Transport.Routing;
    using Skiviez.Wolverine.Exchange.DkimSigner.Properties;

    /// <summary>
    /// Creates new instances of the DkimSigningRoutingAgent.
    /// </summary>
    public sealed class DkimSigningRoutingAgentFactory : RoutingAgentFactory
    {
        /// <summary>
        /// Instance of logger for this class.
        /// </summary>
        private static ILog log = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The algorithm that should be used in signing.
        /// </summary>
        private DkimAlgorithmKind algorithm;

        /// <summary>
        /// The domain that is used when signing.
        /// </summary>
        private string domain;

        /// <summary>
        /// The PEM-encoded private key.
        /// </summary>
        private string encodedKey;

        /// <summary>
        /// The headers to sign in each message.
        /// </summary>
        private IEnumerable<string> headersToSign;

        /// <summary>
        /// The selector to use when signing a message.
        /// </summary>
        private string selector;

        /// <summary>
        /// Initializes a new instance of the <see cref="DkimSigningRoutingAgentFactory"/> class.
        /// </summary>
        public DkimSigningRoutingAgentFactory()
        {
            this.Initialize();

            log.Debug("Creating new instance of DkimSigningRoutingAgentFactory.");
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
            log.Debug("Creating new instance of DkimSigningRoutingAgent.");

            var dkimSigner = new DefaultDkimSigner(
                this.algorithm,
                this.headersToSign,
                this.selector,
                this.domain,
                this.encodedKey);

            return new DkimSigningRoutingAgent(dkimSigner);
        }

        /// <summary>
        /// Initializes various settings based on configuration.
        /// </summary>
        private void Initialize()
        {
            var config = ConfigurationManager.OpenExeConfiguration(
                this.GetType().Assembly.Location);
            var appSettings = config.AppSettings.Settings;

            // Load the signing algorithm.
            try
            {
                this.algorithm = (DkimAlgorithmKind)Enum.Parse(
                    typeof(DkimAlgorithmKind),
                    appSettings["DefaultDkimSigner_Algorithm"].Value,
                    true);
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException(
                    Resources.DkimSigningRoutingAgentFactory_BadAlgorithmConfig,
                    ex);
            }

            // Load the list of headers to sign in each message.
            var unparsedHeaders = appSettings["DefaultDkimSigner_HeadersToSign"].Value;
            if (unparsedHeaders != null)
            {
                this.headersToSign = unparsedHeaders
                    .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }

            // Load the selector that is used when signing messages.
            this.selector = appSettings["DefaultDkimSigner_Selector"].Value;
            if (this.selector == null ||
                this.selector.Length == 0)
            {
                throw new ConfigurationErrorsException(
                    Resources.DkimSigningRoutingAgentFactory_BadSelectorConfig);
            }

            // Load the domain that is used when signing messages.
            this.domain = appSettings["DefaultDkimSigner_Domain"].Value;
            if (this.domain == null ||
                this.domain.Length == 0)
            {
                throw new ConfigurationErrorsException(
                    Resources.DkimSigningRoutingAgentFactory_BadDomainConfig);
            }

            // Load the PEM-encoded private RSA key.
            this.encodedKey = appSettings["DefaultDkimSigner_PrivateKey"].Value;
            if (this.encodedKey == null ||
                this.encodedKey.Length == 0)
            {
                throw new ConfigurationErrorsException(
                    Resources.DkimSigningRoutingAgentFactory_BadPrivateKeyConfig);
            }

            // Initialize Log4Net.
            XmlConfigurator.Configure(new FileInfo(config.FilePath));
        }
    }
}
