using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using DkimSigner.RSA;

namespace Configuration.DkimSigner
{
    public partial class PrivateKeyWindows : Form
    {
        public PrivateKeyWindows(string domain, string selector, string filename = "")
        {
            InitializeComponent();

            this.txtFilename.Text = filename;
            this.txtDnsSelectorRecord.Text = DnsSelector(domain, selector);
        }

        private string DnsSelector(string domain, string selector)
        {
            string record = null;
            try
            {
                record = DNSHelper.GetTxtRecord(selector + "._domainkey." + domain);
                record = record != null ? "Current published DKIM record : \r\n" + record : "No DKIM record is currently publish!";
            }
            catch(Exception)
            {
                record = "Couldn't get current version.\r\nCheck your Internet connexion and restart the application.";
            }

            return record;
        }

        private void btValidate_Click(object sender, EventArgs e)
        {
            if (this.txtFilename.Text != string.Empty && this.txtFilename.Text != null)
                this.Close();
            else
                MessageBox.Show("You must choose a filename.");
        }
    }
}
