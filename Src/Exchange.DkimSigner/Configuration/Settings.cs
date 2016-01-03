using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Exchange.DkimSigner.Configuration
{
    public class Settings
    {
        public int Loglevel { get; set; }
        public DkimAlgorithmKind SigningAlgorithm { get; set; }
        public DkimCanonicalizationKind HeaderCanonicalization { get; set; }
        public DkimCanonicalizationKind BodyCanonicalization { get; set; }

        [XmlArray]
        public List<string> HeadersToSign { get; set; }

        [XmlArray]
        public List<DomainElement> Domains { get; set; } 

        public Settings()
        {
            Loglevel = 3;

            SigningAlgorithm = DkimAlgorithmKind.RsaSha1;
            HeaderCanonicalization = DkimCanonicalizationKind.Simple;
            BodyCanonicalization = DkimCanonicalizationKind.Simple;

            // Don't change because of serializer problem
            //this.HeadersToSign = new List<string>(new string[] { "From", "Subject", "To", "Date", "MessageId" });
            HeadersToSign = new List<string>();

            Domains = new List<DomainElement>();
        }

        /// <summary>
        /// Init settings
        /// </summary>
        public void InitHeadersToSign()
        {
            HeadersToSign = new List<string>(new[] { "From", "Subject", "To", "Date", "MessageId" });
        }

        /// <summary>
        /// Load the config file into this instance
        /// </summary>
        /// <param name="filename">Xml file name</param>
        /// <returns>The true if successfully loaded. False otherwise</returns>
        public bool Load(string filename)
        {            
            if (File.Exists(filename))
            {
                using (Stream stream = File.OpenRead(filename))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                    Settings settings = serializer.Deserialize(stream) as Settings;
                    if (settings == null)
                        return false;

                    Loglevel = settings.Loglevel;
                    SigningAlgorithm = settings.SigningAlgorithm;
                    HeaderCanonicalization = settings.HeaderCanonicalization;
                    BodyCanonicalization = settings.BodyCanonicalization;
                    HeadersToSign = settings.HeadersToSign;
                    Domains = settings.Domains;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Saves to an xml file
        /// </summary>
        /// <param name="filename">File path of the new xml file</param>
        public bool Save(string filename)
        {
            string directory = Path.GetDirectoryName(filename);
            if (directory == null)
            {
                return false;
            }
            if(!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (StreamWriter writer = new StreamWriter(filename))
            {
                XmlSerializer serializer = new XmlSerializer(GetType());
                serializer.Serialize(writer, this);
                writer.Flush();
            }
            return true;
        }
    }
}
