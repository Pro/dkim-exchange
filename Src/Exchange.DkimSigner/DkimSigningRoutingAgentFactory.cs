using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Xml;
using Microsoft.Exchange.Data.Transport;
using Microsoft.Exchange.Data.Transport.Routing;

using ConfigurationSettings;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Exchange.DkimSigner
{
    /// <summary>
    /// Creates new instances of the DkimSigningRoutingAgent.
    /// </summary>
    public sealed class DkimSigningRoutingAgentFactory : RoutingAgentFactory
    {
        /// <summary>
        /// The object that knows how to sign messages.
        /// </summary>
        private ISigner dkimSigner;

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
            return new DkimSigningRoutingAgent(dkimSigner);
        }

        /// <summary>
        /// Initializes various settings based on configuration.
        /// </summary>
        private void Initialize()
        {
            this.dkimSigner = new DkimSigner();
        }
    }
}