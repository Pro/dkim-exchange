using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Xml;
using Microsoft.Exchange.Data.Transport;
using Microsoft.Exchange.Data.Transport.Routing;

using ConfigurationSettings;
using DkimSigner.Properties;

namespace Exchange.DkimSigner
{
    /// <summary>
    /// Creates new instances of the DkimSigningRoutingAgent.
    /// </summary>
    public sealed class DkimSigningRoutingAgentFactory : RoutingAgentFactory
    {
        /// <summary>
        /// The log level defined.
        /// </summary>
        public static int logLevel;

        /// <summary>
        /// The algorithm that should be used in signing.
        /// </summary>
        private DkimAlgorithmKind signingAlgorithm;

        /// <summary>
        /// The canonolization algorithm that should be used for the header.
        /// </summary>
        private DkimCanonicalizationKind headerCanonicalization;

        /// <summary>
        /// The canonolization algorithm that should be used for the body.
        /// </summary>
        private DkimCanonicalizationKind bodyCanonicalization;

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
            var dkimSigner = new DkimSigner(
                this.signingAlgorithm,
                this.headerCanonicalization,
                this.bodyCanonicalization,
                this.headersToSign,
                domainSettings);

            return new DkimSigningRoutingAgent(dkimSigner);
        }
        
        /// <summary>
        /// Initializes various settings based on configuration.
        /// </summary>
        private void Initialize()
        {
            if (RegistryHelper.Open(@"Exchange DkimSigner") != null)
            {
                // Load the log level.
                DkimSigningRoutingAgentFactory.logLevel = 0;
                try
                {
                    string temp = RegistryHelper.Read("LogLevel", @"Exchange DkimSigner");

                    if (temp != null)
                        DkimSigningRoutingAgentFactory.logLevel = Convert.ToInt32(temp);
                }
                catch (FormatException) { }
                catch (OverflowException) { }

                if (logLevel == 0)
                    throw new ConfigurationErrorsException(Resources.DkimSigningRoutingAgentFactory_BadLogLevel);

                // Load the signing algorithm.
                try
                {
                    this.signingAlgorithm = (DkimAlgorithmKind)Enum.Parse(typeof(DkimAlgorithmKind), RegistryHelper.Read("Algorithm", @"Exchange DkimSigner\DKIM"), true);
                }
                catch (Exception ex)
                {
                    throw new ConfigurationErrorsException(Resources.DkimSigningRoutingAgentFactory_BadDkimAlgorithmConfig, ex);
                }

                // Load the header canonicalization algorithm.
                try
                {
                    this.headerCanonicalization = (DkimCanonicalizationKind)Enum.Parse(typeof(DkimCanonicalizationKind), RegistryHelper.Read("HeaderCanonicalization", @"Exchange DkimSigner\DKIM"), true);
                }
                catch (Exception ex)
                {
                    throw new ConfigurationErrorsException(Resources.DkimSigningRoutingAgentFactory_BadDkimCanonicalizationHeaderConfig, ex);
                }

                // Load the body canonicalization algorithm.
                try
                {
                    this.bodyCanonicalization = (DkimCanonicalizationKind)Enum.Parse(typeof(DkimCanonicalizationKind), RegistryHelper.Read("BodyCanonicalization", @"Exchange DkimSigner\DKIM"), true);
                }
                catch (Exception ex)
                {
                    throw new ConfigurationErrorsException(Resources.DkimSigningRoutingAgentFactory_BadDkimCanonicalizationBodyConfig, ex);
                }

                // Load the list of headers to sign in each message.
                string unparsedHeaders = RegistryHelper.Read("HeadersToSign", @"Exchange DkimSigner\DKIM");
                if (unparsedHeaders != null)
                {
                    this.headersToSign = unparsedHeaders.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                }

                // Load the list of domains
                domainSettings = new List<DomainElement>();
                string[] domainNames = RegistryHelper.GetSubKeyName(@"Exchange DkimSigner\Domain");
                if (domainNames != null)
                {
                    foreach (string domainName in domainNames)
                    {
                        string selector = RegistryHelper.Read("Selector", @"Exchange DkimSigner\Domain\" + domainName);
                        string privateKeyFile = RegistryHelper.Read("PrivateKeyFile", @"Exchange DkimSigner\Domain\" + domainName);
                        string recipientRule = RegistryHelper.Read("RecipientRule", @"Exchange DkimSigner\Domain\" + domainName);
                        string senderRule = RegistryHelper.Read("SenderRule", @"Exchange DkimSigner\Domain\" + domainName);

                        DomainElement domainElement = new DomainElement(domainName,
                                                                selector,
                                                                privateKeyFile,
                                                                recipientRule != null ? recipientRule : ".*",
                                                                senderRule != null ? senderRule : ".*");

                        if (domainElement.initElement(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)))
                        {
                            domainSettings.Add(domainElement);
                        }
                    }
                }

                Logger.LogInformation("Exchange DKIM started. Signing Algorithm: " + signingAlgorithm.ToString() + ", Canonicalization Header Algorithm: " + headerCanonicalization.ToString() + ", Canonicalization Header Algorithm: " + bodyCanonicalization.ToString() + ", Number of domains: " + domainSettings.Count);
            }
            else
            {
                throw new ConfigurationErrorsException(Resources.DkimSigningRoutingAgentFactory_BadDkimConfig);
            }
        }
    }
}