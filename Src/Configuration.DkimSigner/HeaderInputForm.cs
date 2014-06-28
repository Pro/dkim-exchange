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
    public partial class HeaderInputForm : Form
    {
        public string header = null;

        public HeaderInputForm()
        {
            InitializeComponent();
        }

        private void HeaderInputForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            header = tbxHeader.Text;
        }

        private void tbxHeader_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter)) //return
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }
    }
}
