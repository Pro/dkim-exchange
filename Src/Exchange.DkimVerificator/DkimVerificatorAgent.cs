using Microsoft.Exchange.Data.Transport;
using Microsoft.Exchange.Data.Transport.Smtp;
using Microsoft.Exchange.Data.Mime;
using Exchange.Dkim;

namespace Exchange.DkimVerificator
{
    /// <summary>
    /// SMTP Receive Agent for DKIM verification
    /// </summary>
    public sealed class DkimVerificatorAgent : SmtpReceiveAgent
    {

        /// <summary>
        /// The object that knows how to verify messages.
        /// </summary>
        private DkimVerificator dkimVerificator;

        public DkimVerificatorAgent(DkimVerificator dkimVerificator)
        {
            this.dkimVerificator = dkimVerificator;

            OnEndOfData += new EndOfDataEventHandler(OnEndOfDataHandler);
        }

        private void OnEndOfDataHandler(ReceiveMessageEventSource source, EndOfDataEventArgs e)
        {
            Logger.LogDebug("Got new message, checking if I verify it...");
            //TODO implement mail verification here!
            //source.RejectMessage(constructSmtpResponse("550", "5.1.1", "Recipient rejected"));
        }

        private static SmtpResponse constructSmtpResponse(string code, string codeDetail, string message)
        {
#if Exchange_2016 || Exchange_2016_RTM || Exchange_2016_CU1 || Exchange_2016_CU2 || Exchange_2016_CU3 || Exchange_2016_CU4
            return SmtpResponse.Create(code, codeDetail, message);
#else
            return new SmtpResponse(code, codeDetail, message);
#endif
        }
    }
}
