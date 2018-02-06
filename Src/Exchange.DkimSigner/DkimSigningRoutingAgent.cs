using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;
using System.Threading;
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

            // get the async context. For an example see: http://www.getcodesamples.com/src/D062E1E9/2552BA7
            // The agent uses the agentAsyncContext object when the agent uses asynchronous execution.
            // The AgentAsyncContext.Complete() method must be invoked
            // before the server will continue processing a message
            agentAsyncContext = GetAgentAsyncContext();

            ThreadPool.QueueUserWorkItem(new WaitCallback(HandleMessageEvent), e.MailItem);

        }

        private void HandleMessageEvent(Object mailItem)
        {
            try
            {
                SignMailItem((MailItem)mailItem);
            }
            catch (Exception ex)
            {
                Logger.LogError("Signing a mail item according to DKIM failed with an exception. Check the logged exception for details.\n" + ex);
            }
            finally
            {
#if !EX_2007_SP3 //not supported in Exchange 2007
                // This allows Transport poison detection to correclty handle this message
                // if there is a crash on this thread.
                agentAsyncContext.Resume();
#endif
                agentAsyncContext.Complete();
                agentAsyncContext = null;
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
                    try
                    {
                        dkimSigner.SignMessage(dkimSigner.GetDomains()[domainPart], mailItem);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("Could not sign message: " + ex.Message);
                    }
                    
                } else {
                    Logger.LogDebug("No entry found in config for domain '" + domainPart + "'");
                }
            }
            else
                Logger.LogDebug("Message is a System message or of TNEF format. Not signing.");
        }
    }
}
