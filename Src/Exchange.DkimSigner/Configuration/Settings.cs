using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ConfigurationSettings
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
            this.Loglevel = 3;

            this.SigningAlgorithm = DkimAlgorithmKind.RsaSha1;
            this.HeaderCanonicalization = DkimCanonicalizationKind.Simple;
            this.BodyCanonicalization = DkimCanonicalizationKind.Simple;

            // Don't change because of serializer problem
            //this.HeadersToSign = new List<string>(new string[] { "From", "Subject", "To", "Date", "Message-ID" });
            this.HeadersToSign = new List<string>();

            this.Domains = new List<DomainElement>();
        }

        /// <summary>
        /// Init settings
        /// </summary>
        public void InitHeadersToSign()
        {
            this.HeadersToSign = new List<string>(new string[] { "From", "Subject", "To", "Date", "Message-ID" });
        }

        /// <summary>
        /// Load the config file
        /// </summary>
        /// <param name="filename">Xml file name</param>
        /// <returns>The object created from the xml file</returns>
        public bool Load(string filename)
        {            
            if (File.Exists(filename))
            {
                using (Stream stream = File.OpenRead(filename))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                    Settings settings = serializer.Deserialize(stream) as Settings;

                    this.Loglevel = settings.Loglevel;
                    this.SigningAlgorithm = settings.SigningAlgorithm;
                    this.HeaderCanonicalization = settings.HeaderCanonicalization;
                    this.BodyCanonicalization = settings.BodyCanonicalization;
                    this.HeadersToSign = settings.HeadersToSign;
                    this.Domains = settings.Domains;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Saves to an xml file
        /// </summary>
        /// <param name="filename">File path of the new xml file</param>
        public void Save(string filename)
        {
            string directory = Path.GetDirectoryName(filename);
            if(!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (StreamWriter writer = new StreamWriter(filename))
            {
                XmlSerializer serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(writer, this);
                writer.Flush();
            }
        }
    }
}
