using System;
using System.Windows.Forms;

namespace Configuration.DkimSigner
{
    public partial class HeaderInputWindows : Form
    {
        /**********************************************************/
        /*********************** Variables ************************/
        /**********************************************************/

        
        /**********************************************************/
        /*********************** Construtor ***********************/
        /**********************************************************/

        public HeaderInputWindows()
        {
            InitializeComponent();
        }

        /**********************************************************/
        /************************* Events *************************/
        /**********************************************************/

        private void HeaderInputForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK && this.txtHeader.Text == string.Empty)
            {
                MessageBox.Show("You must enter a header!", "Value missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        /**********************************************************/
        /******************* Internal functions *******************/
        /**********************************************************/


        /**********************************************************/
        /********************** Button click **********************/
        /**********************************************************/

        private void txtHeader_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter)) //return
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
