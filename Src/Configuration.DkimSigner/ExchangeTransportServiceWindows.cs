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

        public ExchangeTransportServiceWindows(ExchangeServer oServer)
        {
            this.InitializeComponent();

            this.oExchange = oServer;
        }

        /**********************************************************/
        /************************* Events *************************/
        /**********************************************************/

        private void ExchangeTransportServiceWindows_Load(object sender, EventArgs e)
        {
            this.RefreshTransportServiceAgents();

            if(this.oExchange.IsDkimAgentTransportInstalled())
            {
                if (this.oExchange.IsDkimAgentTransportEnabled())
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
                aoAgent = this.oExchange.GetTransportServiceAgents();
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

        /*private void extractAndInstall(string zipPath)
        {
            //TODO: Show extract progress
            string dir = zipPath.Substring(0, zipPath.LastIndexOf('.'));

            try
            {
                Directory.CreateDirectory(dir);
            }
            catch (Exception e)
            {
                MessageBox.Show("Couldn't create directory:\n" + dir + "\n" + e.Message, "Directory error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btInstallUpate.Enabled = true;
                return;
            }

            using (ZipFile zip1 = ZipFile.Read(zipPath))
            {
                foreach (ZipEntry e in zip1)
                {
                    e.Extract(dir, ExtractExistingFileAction.OverwriteSilently);
                }
            }

            string[] contents = Directory.GetDirectories(dir);
            if (contents.Length == 0)
            {
                MessageBox.Show("Downloaded .zip is empty. Please try again.", "Empty download", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btInstallUpate.Enabled = true;
                return;
            }

            string exePath = Path.Combine(contents[0], @"Src\Configuration.DkimSigner\bin\Release\Configuration.DkimSigner.exe");
            if (!File.Exists(exePath))
            {
                MessageBox.Show("Executable not found within downloaded .zip is empty. Please try again.", "Missing .exe", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.btInstallUpate.Enabled = true;
                return;
            }
        }*/

       /*private void performUninstall()
        {
            try
            {
                ExchangeHelper.uninstallTransportAgent();

                if (MessageBox.Show("Transport Agent removed from Exchange. Would you like me to remove all the settings for Exchange DKIM Signer?'", "Remove settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (File.Exists(Path.Combine(Constants.DKIM_SIGNER_PATH, "settings.xml")))
                        File.Delete(Path.Combine(Constants.DKIM_SIGNER_PATH, "settings.xml"));
                }

                if (MessageBox.Show("Transport Agent removed from Exchange. Would you like me to remove the folder '" + Constants.DKIM_SIGNER_PATH + "' and all it's content?", "Remove files?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var dir = new DirectoryInfo(Constants.DKIM_SIGNER_PATH);
                    dir.Delete(true);
                }
            }
            catch (ExchangeHelperException e)
            {
                MessageBox.Show(e.Message, "Uninstall error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.UpdateVersions();
        }*/

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
                /*try
                {
                    Process.Start(exePath, this.btUpdate.Text == "Install" ? "--install" : "--upgrade \"" + Constants.DKIM_SIGNER_PATH + "\"");

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Couldn't start updater:\n" + ex.Message, "Updater error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }*/
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
                    this.oExchange.DisableDkimTransportAgent();
                    this.btDisable.Text = "Enable";
                }
                else
                {
                    this.oExchange.EnableDkimTransportAgent();
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
