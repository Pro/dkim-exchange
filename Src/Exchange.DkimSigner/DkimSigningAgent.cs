using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;
using System.Threading;
using Microsoft.Exchange.Data.Transport;
using Microsoft.Exchange.Data.Transport.Smtp;
namespace Exchange.DkimSigner
{
    /// <summary>
    /// Signs outgoing MIME messages according to the DKIM protocol.
    /// </summary>
    public sealed class DkimSigningAgent : SmtpReceiveAgent
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
        /// Initializes a new instance of the <see cref="DkimSigningAgent"/> class.
        /// </summary>
        /// <param name="dkimSigner">The object that knows how to sign messages.</param>
        /// 
        public DkimSigningAgent(DkimSigner dkimSigner)
        {
            this.dkimSigner = dkimSigner;
            this.OnEndOfData += new EndOfDataEventHandler(this.OnEndOfDataHandler);
        }

        /// <summary>
        /// Fired when Exchange has recieved an end of data command
        /// The OnEndOfDataHandler event is the final event that occurs when a message is being sent via SMTP
        /// best time to sign a message
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="eodArgs">The <see cref="Microsoft.Exchange.Data.Transport.Smtp.EndOfDataEventArgs"/> instance containing the event data.</param>
        [SuppressMessage("Microsoft.Design", "CA1031", Justification = "If an exception is thrown, then the message is eaten by Exchange. Better to catch the exception and write it to a log than to end up with a non-functioning MTA.")]
        public void OnEndOfDataHandler(ReceiveEventSource source, EndOfDataEventArgs eodArgs)
        {
            Logger.LogDebug("Got new message, checking if I can sign it...");

            // get the async context. For an example see: http://www.getcodesamples.com/src/D062E1E9/2552BA7
            // The agent uses the agentAsyncContext object when the agent uses asynchronous execution.
            // The AgentAsyncContext.Complete() method must be invoked
            // before the server will continue processing a message
            agentAsyncContext = GetAgentAsyncContext();

            ThreadPool.QueueUserWorkItem(new WaitCallback(HandleMessageEvent), eodArgs);
   
        }

        private void HandleMessageEvent(Object eodArgs)
        {
            try
            {
#if !EX_2007_SP3 //not supported in Exchange 2007
                // This allows Transport poison detection to correclty handle this message
                // if there is a crash on this thread.
                agentAsyncContext.Resume();
#endif
                SignMailItem((EndOfDataEventArgs)eodArgs);
            }
            catch (Exception ex)
            {
                Logger.LogError("Signing a mail item according to DKIM failed with an exception. Check the logged exception for details.\n" + ex);
            }
            finally
            {
                agentAsyncContext.Complete();
                agentAsyncContext = null;
            }
        }

        /// <summary>
        /// Signs the given mail item, if possible, according to the DKIM standard.
        /// </summary>
        /// <param name="eodArgs">The eodArgs to process containing the MailItem to sign, if possible.</param>
        private void SignMailItem(EndOfDataEventArgs eodArgs)
        {
            // If the mail item is a "system message" then it will be read-only here,
            // and we can't sign it. Additionally, if the message has a "TnefPart",
            // then it is in a proprietary format used by Outlook and Exchange Server,
            // which means we shouldn't bother signing it.
            if (eodArgs.MailItem.Message.IsSystemMessage)
            {
                Logger.LogDebug("Message is a System message. Not signing.");
                return;
            }

            // If this is from an external system, don't sign it
            if (eodArgs.SmtpSession.IsExternalConnection)
            {
                Logger.LogDebug("Message is from an external system. Not signing.");
                return;
            }


            string domainPart = null;
                
            /* Check if we have a valid From address */
            if (!eodArgs.MailItem.FromAddress.IsValid || eodArgs.MailItem.FromAddress.DomainPart == null)
            {
                // The FromAddress is empty. Try to get the domain from somewhere else (see https://github.com/Pro/dkim-exchange/issues/99)
                string smtpAddress = (eodArgs.MailItem.Message != null && eodArgs.MailItem.Message.Sender != null) ? eodArgs.MailItem.Message.Sender.SmtpAddress : null;
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
                    Logger.LogWarning("Invalid from address '" + eodArgs.MailItem.FromAddress + "' and invalid SmtpAddress '" + smtpAddress + "'. Not signing email.");
                    return;
                }
            }
            else
            {
                // from address is valid
                domainPart = eodArgs.MailItem.FromAddress.DomainPart;
            }

            /* If domain was found in define domain configuration */
            if (dkimSigner.GetDomains().ContainsKey(domainPart))
            {
                try
                {
                    dkimSigner.SignMessage(dkimSigner.GetDomains()[domainPart], eodArgs.MailItem);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Could not sign message: " + ex.Message);
                }
                    
            } else {
                Logger.LogDebug("No entry found in config for domain '" + domainPart + "'");
            }
            
                
        }
    }
}
