using System;
using Microsoft.Exchange.Data.Transport;
using Microsoft.Exchange.Data.Transport.Smtp;
using Microsoft.Exchange.Data.Mime;

using Exchange.Dkim;
using Exchange.DkimSigner.Configuration;
using System.IO;
using MimeKit;
using System.Text;

namespace Exchange.DkimVerificator
{
    public class DkimVerificator : SettingsReceiver
    {
        /// <summary>
        /// Object used as a mutex when settings are updated during execution
        /// </summary>
        private object settingsMutex;

        private PublicKeyLocator keyLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DkimVerificator"/> class.
        /// </summary>
        public DkimVerificator()
        {
            settingsMutex = new object();
            keyLocator = new PublicKeyLocator();
        }


        public void UpdateSettings(Settings config)
        {

            lock (settingsMutex)
            {
                throw new NotImplementedException();
            }
        }

        public bool VerifyMessage(MailItem mailItem)
        {
            bool verifyResult = false;
            using (Stream stream = GetRawMessageStream(mailItem))
            {
                stream.Seek(0, SeekOrigin.Begin);

                Logger.LogDebug("Parsing the MimeMessage");
                MimeMessage message = MimeMessage.Load(stream, true);

                Logger.LogDebug("Verifying the message");
                int index = message.Headers.IndexOf(MimeKit.HeaderId.DkimSignature);
                lock (settingsMutex)
                {
                    // a message may have multiple DKIM headers, we have to check for the first successful validation
                    foreach (MimeKit.Header h in message.Headers)
                    {
                        if (h.Id != MimeKit.HeaderId.DkimSignature)
                            continue;
                        verifyResult = message.Verify(h, keyLocator);
                        if (verifyResult)
                            break;
                    }
                }

                if (verifyResult)
                {
                    Logger.LogDebug("DKIM signature is valid");
                }
                else
                {
                    Logger.LogWarning("DKIM signature did not verify.");
                }

                stream.Close();
            }
            return verifyResult;
        }

        // Transport decodes message headers when creating MailItem object
        // This function will get the raw message headers and body and create a stream for MimeKit
        private Stream GetRawMessageStream(MailItem mailItem)
        {
            StringBuilder builder = new StringBuilder();

            using (MemoryStream stream = new MemoryStream())
            {
                EncodingOptions options = new EncodingOptions("US-ASCII", "en-US", EncodingFlags.None);

                // Get the raw message headers and convert to string
                mailItem.Message.RootPart.Headers.WriteTo(stream, options);
                string messageHeaders = StreamToString(stream);
                builder.Append(messageHeaders);
                builder.AppendLine("");
            }

            // Get the message body and convert to string
            string messageBody = StreamToString(mailItem.Message.MimeDocument.RootPart.GetRawContentReadStream());
            builder.Append(messageBody);

            // Merge headers and body to string
            string eml = builder.ToString();

            return StringToStream(eml);
        }

        private string StreamToString(Stream stream)
        {
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        private Stream StringToStream(string src)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(src);
            return new MemoryStream(byteArray);
        }
    }
}
