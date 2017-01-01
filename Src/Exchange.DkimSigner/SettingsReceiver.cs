using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exchange.DkimSigner.Configuration;

namespace Exchange.Dkim
{
    interface SettingsReceiver
    {
        void UpdateSettings(Settings config);
    }
}
