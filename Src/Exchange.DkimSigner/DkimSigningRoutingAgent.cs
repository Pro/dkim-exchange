namespace Exchange.DkimSigner
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.IO;
    using Microsoft.Exchange.Data.Transport;
    using Microsoft.Exchange.Data.Transport.Routing;
    using Exchange.DkimSigner.Properties;

    /// <summary>
    /// Signs outgoing MIME messages according to the DKIM protocol.
    /// </summary>
    public sealed class DkimSigningRoutingAgent : RoutingAgent
    {

        /// <summary>
        /// The object that knows how to sign messages.
        /// </summary>
        private IDkimSigner dkimSigner;

        /// <summary>
        /// Initializes a new instance of the <see cref="DkimSigningRoutingAgent"/> class.
        /// </summary>
        /// <param name="dkimSigner">The object that knows how to sign messages.</param>
        public DkimSigningRoutingAgent(IDkimSigner dkimSigner)
        {
            if (dkimSigner == null)
            {
                throw new ArgumentNullException("dkimSigner");
            }

            this.dkimSigner = dkimSigner;
            
            this.OnCategorizedMessage += this.WhenMessageCategorized;
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
        /// <param name="e">The 
        /// <see cref="Microsoft.Exchange.Data.Transport.Routing.QueuedMessageEventArgs"/> 
        /// instance containing the event data.</param>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1031",
            Justification = "If an exception is thrown, then the message is eaten " +
                "by Exchange. Better to catch the exception and write it to a log " +
                "than to end up with a non-functioning MTA.")]
        private void WhenMessageCategorized(
            CategorizedMessageEventSource source,
            QueuedMessageEventArgs e)
        {
            try
            {
                this.SignMailItem(e.MailItem);
            }
            catch (Exception ex)
            {
                Logger.LogError(Resources.DkimSigningRoutingAgent_SignFailed + "\n" + ex.ToString());
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

            if (!mailItem.Message.IsSystemMessage &&
                mailItem.Message.TnefPart == null)
            {
                using (var inputStream = mailItem.GetMimeReadStream())
                {
                    string dkim = this.dkimSigner.CanSign(inputStream);

                    if (dkim.Length != 0)
                    {
                        string source = this.dkimSigner.SourceMessage(inputStream);

                        inputStream.Close();

                        Logger.LogInformation("Signing mail with header: " + dkim);

                        using (var outputStream = mailItem.GetMimeWriteStream())
                        {
                            try
                            {
                                this.dkimSigner.Sign(source, outputStream, dkim);
                            }
                            catch (Exception ex)
                            {
                                Logger.LogError("Signing went terribly wrong: " + ex.ToString());
                            }

                            outputStream.Close();
                        }
                    }
                }
            }
        }
    }
}
