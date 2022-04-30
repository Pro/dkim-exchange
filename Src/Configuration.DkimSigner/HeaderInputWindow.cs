using System;
using System.Windows.Forms;

namespace Configuration.DkimSigner
{
	public partial class HeaderInputWindow : Form
	{
		// ##########################################################
		// ##################### Variables ##########################
		// ##########################################################

		// ##########################################################
		// ##################### Construtor #########################
		// ##########################################################

		public HeaderInputWindow()
		{
			InitializeComponent();


			cbxHeader.AutoCompleteCustomSource.AddRange(Enum.GetNames(typeof(MimeKit.HeaderId)));
		}

		// ##########################################################
		// ####################### Events ###########################
		// ##########################################################

		private void HeaderInputForm_FormClosing(object sender, FormClosingEventArgs e)
		{

			if (DialogResult == DialogResult.OK && cbxHeader.Text == string.Empty)
			{
				MessageBox.Show(this, "You must enter a header!", "Value missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
				e.Cancel = true;
			}
		}

		public string GetHeaderName()
		{
			return cbxHeader.Text;
		}

		// ##########################################################
		// ################# Internal functions #####################
		// ##########################################################

		// ###########################################################
		// ###################### Button click #######################
		// ###########################################################

		private void cbxHeader_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == Convert.ToChar(Keys.Enter)) //return
			{
				DialogResult = DialogResult.OK;
				Close();
			}
		}
	}
}