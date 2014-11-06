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
            Loglevel = 3;
            SigningAlgorithm = DkimAlgorithmKind.RsaSha1;
            HeaderCanonicalization = DkimCanonicalizationKind.Relaxed;
            BodyCanonicalization = DkimCanonicalizationKind.Relaxed;
            HeadersToSign = new List<string>(new string[] { "From", "Subject", "To", "Date", "Message-ID" });
            Domains = new List<DomainElement>();
        }

        /// <summary>
        /// Load the config file
        /// </summary>
        /// <param name="filename">Xml file name</param>
        /// <returns>The object created from the xml file</returns>
        public void Load(string filename)
        {
            using (var stream = File.OpenRead(filename))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                Settings settings = serializer.Deserialize(stream) as Settings;

                this.Loglevel = settings.Loglevel;
                this.SigningAlgorithm = settings.SigningAlgorithm;
                this.HeaderCanonicalization = settings.HeaderCanonicalization;
                this.BodyCanonicalization = settings.BodyCanonicalization;
                this.HeadersToSign = settings.HeadersToSign;
                this.Domains = settings.Domains;
            }
        }

        /// <summary>
        /// Saves to an xml file
        /// </summary>
        /// <param name="filename">File path of the new xml file</param>
        public void Save(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                XmlSerializer serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(writer, this);
                writer.Flush();
            }
        }
    }
}