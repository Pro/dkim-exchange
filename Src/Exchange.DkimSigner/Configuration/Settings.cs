using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ConfigurationSettings
{
    public class Settings
    {
        public Settings()
        {
            // default settings
            Loglevel = 3; //Information
            SigningAlgorithm = DkimAlgorithmKind.RsaSha1;
            HeaderCanonicalization = DkimCanonicalizationKind.Relaxed;
            BodyCanonicalization = DkimCanonicalizationKind.Relaxed;
            HeadersToSign = new List<string>();//new string[] {"From","Subject","To","Date","Message-ID"});
            Domains = new List<DomainElement>();
        }

        public int Loglevel { get; set; }
        public DkimAlgorithmKind SigningAlgorithm { get; set; }
        public DkimCanonicalizationKind HeaderCanonicalization { get; set; }
        public DkimCanonicalizationKind BodyCanonicalization { get; set; }

        [XmlArray]
        public List<string> HeadersToSign { get; set; } 

        [XmlArray]
        public List<DomainElement> Domains { get; set; } 

        /// <summary>
        /// Load the config file
        /// </summary>
        /// <param name="FileName">Xml file name</param>
        /// <returns>The object created from the xml file</returns>
        public static Settings Load(string FileName)
        {
            using (var stream = System.IO.File.OpenRead(FileName))
            {
                var serializer = new XmlSerializer(typeof(Settings));
                return serializer.Deserialize(stream) as Settings;
            }
        }

        public static Settings LoadOrCreate(string FileName)
        {
            if (!System.IO.File.Exists(FileName))
            {
                // default settings
                Settings s = new Settings();
                s.HeadersToSign = new List<string>(new string[] {"From","Subject","To","Date","Message-ID"});
                return s;
            }
            else
            {
                return Load(FileName);
            }
        }

        /// <summary>
        /// Saves to an xml file
        /// </summary>
        /// <param name="FileName">File path of the new xml file</param>
        public void Save(string FileName)
        {
            using (var writer = new System.IO.StreamWriter(FileName))
            {
                var serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(writer, this);
                writer.Flush();
            }
        }

    }
}
