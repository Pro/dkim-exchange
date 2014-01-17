using ConfigurationSettings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ConfigurationSettings
{
    public class DomainSection : ConfigurationSection
    {
        [ConfigurationProperty("Domains")]
        public DomainCollection Domains
        {
            get { return ((DomainCollection)(base["Domains"])); }
            set { base["Domains"] = value; }
        }
    }
}
