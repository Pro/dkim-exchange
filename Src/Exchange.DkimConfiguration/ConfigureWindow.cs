using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Exchange.DkimConfiguration
{
    public partial class ConfigureWindow : Form
    {
        public ConfigureWindow()
        {
            InitializeComponent();
        }

        private void ConfigureWindow_Load(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, string> entry in Constants.DkimSignerVersionDirectory)
            {
                dgvExchangeVersion.Rows.Add(entry.Key, entry.Value);
            }
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
