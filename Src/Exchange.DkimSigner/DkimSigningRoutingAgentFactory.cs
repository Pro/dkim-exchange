namespace Exchange.DkimSigner
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Reflection;
    using Microsoft.Exchange.Data.Transport;
    using Microsoft.Exchange.Data.Transport.Routing;
    using Exchange.DkimSigner.Properties;
    using Exchange.DkimSigner.Config;
    using System.Xml;

    /// <summary>
    /// Creates new instances of the DkimSigningRoutingAgent.
    /// </summary>
    public sealed class DkimSigningRoutingAgentFactory : RoutingAgentFactory
    {
        /// <summary>
        /// The algorithm that should be used in signing.
        /// </summary>
        private DkimAlgorithmKind algorithm;

        /// <summary>
        /// The headers to sign in each message.
        /// </summary>
        private IEnumerable<string> headersToSign;

        /// <summary>
        /// The list of domains loaded from config file.
        /// </summary>
        private List<DomainElement> domainSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="DkimSigningRoutingAgentFactory"/> class.
        /// </summary>
        public DkimSigningRoutingAgentFactory()
        {
            this.Initialize();

            Logger.LogInformation("Creating new instance of DkimSigningRoutingAgentFactory.");
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
            Logger.LogInformation("Creating new instance of DkimSigningRoutingAgent.");

            var dkimSigner = new DefaultDkimSigner(
                this.algorithm,
                this.headersToSign,
                domainSettings);

            return new DkimSigningRoutingAgent(dkimSigner);
        }
        
        /// <summary>
        /// Initializes various settings based on configuration.
        /// </summary>
        private void Initialize()
        {
            // Load the signing algorithm.
            try
            {
                this.algorithm = (DkimAlgorithmKind)Enum.Parse(
                    typeof(DkimAlgorithmKind),
                    Properties.Settings.Default.Algorithm,
                    true);
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException(
                    Resources.DkimSigningRoutingAgentFactory_BadAlgorithmConfig,
                    ex);
            }

            // Load the list of headers to sign in each message.
            var unparsedHeaders = Properties.Settings.Default.HeadersToSign;
            if (unparsedHeaders != null)
            {
                this.headersToSign = unparsedHeaders
                    .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }

            try
            {
                XmlDocument xmlDoc = new XmlDocument(); //* create an xml document object.
                xmlDoc.Load(this.GetType().Assembly.Location + ".config"); //* load the XML document from the specified file.
                XmlNodeList domainInfoList = xmlDoc.GetElementsByTagName("domainInfo");
                if (domainInfoList == null || domainInfoList.Count != 1)
                {
                    Logger.LogError("There is an error in your configuration file. domainInfo couldn't be initialized properly.");
                    return;
                }
                XmlNode domainInfo = domainInfoList.Item(0);

                domainSettings = new List<DomainElement>();

                foreach (XmlNode n in domainInfo.ChildNodes)
                {
                    DomainElement e = new DomainElement();
                    e.Domain = n.Attributes["Domain"].Value;
                    e.Selector = n.Attributes["Selector"].Value;
                    e.PrivateKeyFile = n.Attributes["PrivateKeyFile"].Value;
                    if (e.initElement(Path.GetDirectoryName(this.GetType().Assembly.Location)))
                        domainSettings.Add(e);
                }
                if (domainSettings.Count == 0)
                {
                    Logger.LogWarning("No domain configuration found. DKIM will do nothing.");
                }
            }
            catch (Exception e)
            {
                Logger.LogError("Couldn't load config: " + e.ToString());
            }
            Logger.LogInformation("Exchange DKIM started. Algorithm: " + algorithm.ToString() + " Number of domains: " + domainSettings.Count);
        }
    }
}
