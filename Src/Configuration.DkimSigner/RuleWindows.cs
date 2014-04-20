using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Configuration.DkimSigner
{
    public partial class RuleWindows : Form
    {
        public RuleWindows(string recipientRule = "", string senderRule = "")
        {
            InitializeComponent();

            this.txtRecipientRule.Text = recipientRule;
            this.txtSenderRule.Text = senderRule;
        }

        private void btValidate_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
