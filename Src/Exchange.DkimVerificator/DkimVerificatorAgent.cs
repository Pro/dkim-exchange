using Microsoft.Exchange.Data.Transport.Smtp;
using Exchange.Dkim;
using System;
using Microsoft.Exchange.Data.Mime;

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

        private string serverName;

        public DkimVerificatorAgent(string serverName, DkimVerificator dkimVerificator)
        {
            this.dkimVerificator = dkimVerificator;
            this.serverName = serverName;

            OnEndOfData += new EndOfDataEventHandler(OnEndOfDataHandler);
        }

        private void OnEndOfDataHandler(ReceiveMessageEventSource source, EndOfDataEventArgs e)
        {
            Logger.LogDebug("Got new message, checking if I can verify it...");
            try
            {
                // If the mail item is a "system message" then it will be read-only here,
                // and we should not verify it. Additionally, if the message has a "TnefPart",
                // then it is in a proprietary format used by Outlook and Exchange Server,
                // which means we shouldn't bother verifying it.
                if (!e.MailItem.Message.IsSystemMessage && e.MailItem.Message.TnefPart == null)
                {
                    string result = "fail";
                    if (dkimVerificator.VerifyMessage(e.MailItem))
                    {
                        result = "pass";
                    }
                    string authResults = "dkim=" + result + ";";
                    MimeDocument mdMimeDoc = e.MailItem.Message.MimeDocument;
                    HeaderList hlHeaderlist = mdMimeDoc.RootPart.Headers;

                    Header authResultsHeader = hlHeaderlist.FindFirst("Authentication-Results");
                    if (authResultsHeader == null) { 
                        
                        TextHeader nhNewHeader = new TextHeader("Authentication-Results", serverName + "; " + authResults);

                        MimeNode dkimSignatureHeader = hlHeaderlist.FindFirst("DKIM-Signature");

                        if (dkimSignatureHeader == null)
                        {
                            dkimSignatureHeader = hlHeaderlist.LastChild;
                        }
                        hlHeaderlist.InsertBefore(nhNewHeader, dkimSignatureHeader);
                    }
                    else
                    {
                        authResultsHeader.Value += "; " + authResults;
                    }
                }
                else
                    Logger.LogDebug("Message is a System message or of TNEF format. Not verifying.");
            }
            catch (Exception ex)
            {
                Logger.LogError("Verifying a mail item according to DKIM failed with an exception. Check the logged exception for details.\n" + ex);
            }
        }

    }
}
