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
        /**********************************************************/
        /*********************** Variables ************************/
        /**********************************************************/

        
        /**********************************************************/
        /*********************** Construtor ***********************/
        /**********************************************************/

        public HeaderInputForm()
        {
            InitializeComponent();
        }

        /**********************************************************/
        /************************* Events *************************/
        /**********************************************************/


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
