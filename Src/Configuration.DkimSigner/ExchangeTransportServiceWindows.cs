using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Configuration.DkimSigner.Exchange;
using Ionic.Zip;

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

            try
            {
                Process.Start(exePath, this.btInstallUpate.Enabled == true && this.btInstallUpate.Text == "Install" ? "--install" : "--upgrade \"" + Constants.DKIM_SIGNER_PATH + "\"");

                this.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Couldn't start updater:\n" + e.Message, "Updater error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.btInstallUpate.Enabled = true;
            }
        }*/

        /*private void performUpgrade()
        {
            if (dkimSignerAvailable == null)
                return;

            if (!CheckSaveConfig())
            {
                return;
            }

            this.btInstallUpate.Enabled = false;

            string tempDir = Path.GetTempPath() + Guid.NewGuid().ToString();
            string tempPath = tempDir + ".zip";
            DownloadProgressWindow dpw = new DownloadProgressWindow(dkimSignerAvailable.ZipballUrl, tempPath);

            if (dpw.ShowDialog() == DialogResult.OK)
            {
                extractAndInstall(tempPath);
            }
        }*/

       /*private void performUninstall()
        {
            try
            {
                ExchangeHelper.uninstallTransportAgent();

                if (MessageBox.Show("Transport Agent removed from Exchange. Would you like me to remove all the settings for Exchange DKIM Signer?'", "Remove settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (File.Exists(Path.Combine(Constants.DKIM_SIGNER_PATH, "settings.xml")))
                        File.Delete(Path.Combine(Constants.DKIM_SIGNER_PATH, "settings.xml"));
                }

                if (MessageBox.Show("Transport Agent removed from Exchange. Would you like me to remove the folder '" + Constants.DKIM_SIGNER_PATH + "' and all it's content?", "Remove files?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /*private void btRestartTransportService_Click(object sender, EventArgs e)
        {
            this.lblExchangeStatus.Text = "Restarting ...";
            Application.DoEvents();
            this.restartTransportService();
        }*/

        /// <summary>
        /// Update the current Configuration.ExchangeDkimSigner WindowsForm and the Exchange.DkimSigner transport agent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /*private void btInstallUpate_Click(object sender, EventArgs e)
        {
            if (this.btInstallUpate.Enabled == true)
            {
                switch (this.btInstallUpate.Text)
                {
                    case "Downgrade":
                        if (MessageBox.Show("Do you really want to downgrade the DKIM Exchange Agent from Version " + dkimSignerInstalled.ToString() + " to " + dkimSignerAvailable.Version.ToString() + "?", "Downgrade?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.performUpgrade();
                        }
                        break;
                    case "Upgrade":
                        if (MessageBox.Show("Do you really want to upgrade the DKIM Exchange Agent from Version " + dkimSignerInstalled.ToString() + " to " + dkimSignerAvailable.Version.ToString() + "?", "Upgrade?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.performUpgrade();
                        }
                        break;
                    case "Install":
                        if (MessageBox.Show("Do you really want to install the DKIM Exchange Agent version " + dkimSignerAvailable.Version.ToString() + "?", "Install?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.performUpgrade();
                        }
                        break;
                }
            }
        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /*private void btInstallZip_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFileDialog = new OpenFileDialog();

            oFileDialog.FileName = "dkim-exchange.zip";
            oFileDialog.Filter = "ZIP files|*.zip";
            oFileDialog.Title = "Select the .zip file downloaded from github.com";

            if (oFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.extractAndInstall(oFileDialog.FileName);
            }
        }*/

        private void btRefresh_Click(object sender, EventArgs e)
        {
            this.RefreshTransportServiceAgents();
        }

        private void btUpdate_Click(object sender, EventArgs e)
        {
            if(this.btUpdate.Text == "Install")
            {

            }
            else
            {

            }
        }

        private void btUninstall_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to UNINSTALL the DKIM Exchange Agent?\nPlease remove the following folder manually:\n" + Constants.DKIM_SIGNER_PATH, "Uninstall?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
            catch (ExchangeHelperException ex)
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
