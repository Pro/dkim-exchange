using System;
using System.Collections.Generic;

namespace Configuration.DkimSigner
{
    public static class Constants
    {
        public static string DkimSignerPath
        {
            get { return string.Format("{0}\\Exchange DkimSigner", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).TrimEnd('\\')); }
        }

        public const string DkimSignerAgentName = @"Exchange DkimSigner";
        public const string DkimSignerAgentDll = @"ExchangeDkimSigner.dll";
        public const string DkimSignerConfigurationExe = @"Configuration.DkimSigner.exe";

        public const string DkimSignerNotice = "This is free software; see the source for copying conditions. There is NO warranty; not even for MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.";
        public const string DkimSignerLicence = "This Software is provided under the GNU GPLv3 License.\r\nFuther details can be found here: http://www.gnu.org/licenses";
        public const string DkimSignerAuthor = "The code for the signing algorithm is based on the work of Nicholas Piasecki.\r\n\r\nDeveloped and maintained by Stefan Profanter and Alexandre Laroche.";
        public const string DkimSignerWebsite = "Website : https://github.com/Pro/dkim-exchange";

        public const string DkimSignerEventlogSource = @"Exchange DKIM";
        public const string DkimSignerEventlogRegistry = @"SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM";

        public static readonly Dictionary<string, string> DkimSignerVersionDirectory = new Dictionary<string, string>
        {
            // RU21 has newer .dlls than older SP3
            {"8.3.298.", "Exchange 2007 SP3 RU21"},
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
            {"15.0.1156.6", "Exchange 2013 SP1 CU11"},
            {"15.0.1178.4", "Exchange 2013 SP1 CU12"},
            {"15.0.1210.3", "Exchange 2013 SP1 CU13"},
            {"15.0.1236.3", "Exchange 2013 SP1 CU14"},
            {"15.0.1263.5", "Exchange 2013 SP1 CU15"},
            {"15.0.1293.2", "Exchange 2013 SP1 CU16"},
            {"15.0.1320.4", "Exchange 2013 SP1 CU17"},
            {"15.0.1347.2", "Exchange 2013 SP1 CU18"},
            {"15.0.1365.1", "Exchange 2013 SP1 CU19"},
            {"15.0.1367.3", "Exchange 2013 SP1 CU20"},
            {"15.0.1395.4", "Exchange 2013 SP1 CU21"},
            {"15.1.225.17", "Exchange 2016 Preview"},
            {"15.1.225.42", "Exchange 2016 RTM"},
            {"15.1.396.30", "Exchange 2016 CU1"},
            {"15.1.466.34", "Exchange 2016 CU2"},
            {"15.1.544.27", "Exchange 2016 CU3"},
            {"15.1.669.32", "Exchange 2016 CU4"},
            {"15.1.845.34", "Exchange 2016 CU5"},
            {"15.1.1034.26", "Exchange 2016 CU6"},
            {"15.1.1261.35", "Exchange 2016 CU7"},
            {"15.1.1415.2", "Exchange 2016 CU8"},
            {"15.1.1466.3", "Exchange 2016 CU9"},
            {"15.1.1531.3", "Exchange 2016 CU10" }
        };
    }
}
