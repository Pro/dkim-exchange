using System.Collections.Generic;

namespace Configuration.DkimSigner
{
    public static class Constants
    {
        public const string DKIM_SIGNER_PATH = @"C:\Program Files\Exchange DkimSigner";
        public const string DKIM_SIGNER_AGENT_NAME = @"Exchange DkimSigner";
        public const string DKIM_SIGNER_AGENT_DLL = @"ExchangeDkimSigner.dll";
        public const string DKIM_SIGNER_CONFIGURATION_EXE = @"Configuration.DkimSigner.exe";

        public const string DKIM_SIGNER_NOTICE = "This is free software; see the source for copying conditions. There is NO warranty; not even for MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.";
        public const string DKIM_SIGNER_LICENCE = "This Software is provided under the GNU GPLv3 License.\r\nFuther details can be found here: http://www.gnu.org/licenses";
        public const string DKIM_SIGNER_AUTHOR = "The code for the signing algorithm is based on the work of Nicholas Piasecki.\r\n\r\nDeveloped and maintained by Stefan Profanter and Alexandre Laroche.";
        public const string DKIM_SIGNER_WEBSITE = "Website : https://github.com/Pro/dkim-exchange";

        public const string DKIM_SIGNER_EVENTLOG_SOURCE = @"Exchange DKIM";
        public const string DKIM_SIGNER_EVENTLOG_REGISTRY = @"SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM";

        public static readonly Dictionary<string, string> DKIM_SIGNER_VERSION_DIRECTORY = new Dictionary<string, string>
        {
            {"8.3.", "Exchange 2007 SP3"},
            {"14.0.", "Exchange 2010"},
            {"14.1.", "Exchange 2010 SP1"},
            {"14.2.", "Exchange 2010 SP2"},
            {"14.3.", "Exchange 2010 SP3"},
            {"15.0.516.32", "Exchange 2013"},
            {"15.0.620.29", "Exchange 2013 CU1"},
            {"15.0.712.24", "Exchange 2013 CU2"},
            {"15.0.775.38", "Exchange 2013 CU3"},
            {"15.0.847.32", "Exchange 2013 SP1 CU4"},
            {"15.0.913.22", "Exchange 2013 SP1 CU5"},
            {"15.0.995.29", "Exchange 2013 SP1 CU6"},
            {"15.0.1044.25", "Exchange 2013 SP1 CU7"},
            {"15.0.1076.9", "Exchange 2013 SP1 CU8"},
            {"15.0.1104.5", "Exchange 2013 SP1 CU9"},
            {"15.0.1130.7", "Exchange 2013 SP1 CU10"},
            {"15.1.225.17", "Exchange 2016 Preview"},
            {"15.1.225.42", "Exchange 2016 RTM"}
        };
    }
}
