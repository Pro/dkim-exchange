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
    public partial class RuleWindow : Form
    {
        /// <summary>
        /// Constructor for the RuleWindows
        /// </summary>
        /// <param name="recipientRule">Optional regex that sign the all mails just for specific recipients for the domain</param>
        /// <param name="senderRule">Optional regex that sign the all mails just for specific senders for the domain</param>
        public RuleWindow(string recipientRule = "", string senderRule = "")
        {
            InitializeComponent();

            this.txtRecipientRule.Text = recipientRule;
            this.txtSenderRule.Text = senderRule;
        }

        /// <summary>
        /// Exit the windows form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btValidate_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
