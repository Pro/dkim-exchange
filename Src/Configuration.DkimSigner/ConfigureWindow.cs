using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Configuration.DkimSigner
{
    public partial class ConfigureWindow : Form
    {
        public ConfigureWindow()
        {
            this.InitializeComponent();
        }

        private void ConfigureWindow_Load(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, string> entry in Constants.DKIM_SIGNER_VERSION_DIRECTORY)
            {
                this.dgvExchangeVersion.Rows.Add(entry.Key, entry.Value);
            }
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
