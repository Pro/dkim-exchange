using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ConfigurationSettings
{
    public class General : ConfigurationSection
    {
        [ConfigurationProperty("LogLevel", DefaultValue = 2, IsKey = false, IsRequired = true)]
        [IntegerValidator(ExcludeRange = false, MaxValue = 3, MinValue = 0)]
        public int LogLevel
        {
            get
            {
                return (int)this["LogLevel"];
            }
            set
            {
                this["LogLevel"] = value;
            }
        }

        [ConfigurationProperty("HeadersToSign", DefaultValue = "From; Subject; To; Date; Message-ID;", IsKey = false, IsRequired = true)]
        public string HeadersToSign
        {
            get
            {
                return (string)this["HeadersToSign"];
            }
            set
            {
                this["HeadersToSign"] = value;
            }
        }

        [ConfigurationProperty("Algorithm", DefaultValue = "RsaSha1", IsKey = false, IsRequired = true)]
        public string Algorithm
        {
            get
            {
                return (string)this["Algorithm"];
            }
            set
            {
                this["Algorithm"] = value;
            }
        }


    }
}
