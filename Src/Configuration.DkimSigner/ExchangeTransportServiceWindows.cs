using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Configuration.DkimSigner.Exchange;

namespace Configuration.DkimSigner
{
    public partial class ExchangeTransportServiceWindows : Form
    {
        /**********************************************************/
        /*********************** Variables ************************/
        /**********************************************************/
        private ExchangeServer oExchange = null;

        /**********************************************************/
        /*********************** Construtor ***********************/
        /**********************************************************/

        public ExchangeTransportServiceWindows()
        {
            this.InitializeComponent();
        }

        /**********************************************************/
        /************************* Events *************************/
        /**********************************************************/

        private void ExchangeTransportServiceWindows_Load(object sender, EventArgs e)
        {
            this.RefreshTransportServiceAgents();

            if(ExchangeServer.IsDkimAgentTransportInstalled())
            {
                if (ExchangeServer.IsDkimAgentTransportEnabled())
                {
                    this.btDisable.Text = "Enable";
                }
            }
            else
            {
                this.btUpdate.Text = "Install";
                this.btDisable.Text = "Enable";

                this.btUninstall.Enabled = false;
                this.btDisable.Enabled = false;
                this.btMoveUp.Enabled = false;
                this.btMoveDown.Enabled = false;
            }
        }

        /**********************************************************/
        /******************* Internal functions *******************/
        /**********************************************************/

        private void RefreshTransportServiceAgents()
        {
            List<TransportServiceAgent> aoAgent = null;

            try
            {
                aoAgent = ExchangeServer.GetTransportServiceAgents();
            }
            catch(Exception) { }

            this.dgvTransportServiceAgents.Rows.Clear();

            if (aoAgent != null)
            {
                foreach (TransportServiceAgent oAgent in aoAgent)
                {
                    this.dgvTransportServiceAgents.Rows.Add(oAgent.Priority, oAgent.Name, oAgent.Enabled);
                }
            }
        }

        /**********************************************************/
        /********************** Button click **********************/
        /**********************************************************/

        private void btRefresh_Click(object sender, EventArgs e)
        {
            this.RefreshTransportServiceAgents();
        }

        private void btUpdate_Click(object sender, EventArgs e)
        {
            if (this.btUpdate.Text == "Update" ? MessageBox.Show("Do you really want to UPDATE the DKIM Exchange Agent?\n", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes : true)
            {
                try
                {
                    System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location, this.btUpdate.Text == "Install" ? "--install" : "--upgrade");

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Couldn't start the process :\n" + ex.Message, "Updater error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btUninstall_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to UNINSTALL the DKIM Exchange Agent?\n", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //this.performUninstall();
            }
        }

        private void btDisable_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.btDisable.Text == "Disable")
                {
                    ExchangeServer.DisableDkimTransportAgent();
                    this.btDisable.Text = "Enable";
                }
                else
                {
                    ExchangeServer.EnableDkimTransportAgent();
                    this.btDisable.Text = "Disable";
                }
            }
            catch (ExchangeServerException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btMoveUp_Click(object sender, EventArgs e)
        {

        }

        private void btMoveDown_Click(object sender, EventArgs e)
        {

        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
