using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Configuration.DkimSigner
{
    public partial class PrivateKeyWindows : Form
    {
        public PrivateKeyWindows(string domain, byte[] key, string filename = "")
        {
            InitializeComponent();

            this.txtFilename.Text = filename;
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
