using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Configuration.DkimSigner
{
    public partial class UpgradeWindow : Form
    {
        /**********************************************************/
        /*********************** Variables ************************/
        /**********************************************************/

        DialogResult dialogResult;
        string tempPath;
        string installPath;

        /**********************************************************/
        /*********************** Construtor ***********************/
        /**********************************************************/

        public UpgradeWindow(string tempPath, string installPath)
        {
            InitializeComponent();

            this.tempPath = tempPath;
            this.installPath = installPath;
            dialogResult = DialogResult.Cancel;
        }

        /**********************************************************/
        /************************* Events *************************/
        /**********************************************************/

        private void startUpgrade()
        {
            resetStatus();
            stopServiceTask();
        }

        private string installAgent()
        {
            try
            {
                bool enabled;
                bool installed = ExchangeHelper.isAgentInstalled(out enabled);
                if (installed && enabled)
                {
                    return null;
                }

                if (!installed)
                {
                    ExchangeHelper.installTransportAgent();
                }

                ExchangeHelper.enalbeTransportAgent();
                return null;
            }
            catch (ExchangeHelperException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void installAgentTask()
        {
            lbInstallAgent.Enabled = true;

            Task.Factory.StartNew(() => installAgent()).ContinueWith(task => installAgentResult(task.Result), TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void installAgentResult(string ret)
        {
            if (ret == null)
            {
                picInstallAgent.Image = statusImageList.Images[0];
                Application.DoEvents();
                startServiceTask();
            }
            else
            {
                picInstallAgent.Image = statusImageList.Images[1];
                MessageBox.Show(ret, "Install Agent error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                done(false);
            }
        }

        private string startService()
        {
            try
            {
                ExchangeHelper.startTransportService();
                return null;
            }
            catch (ExchangeHelperException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void startServiceTask()
        {
            lbStartService.Enabled = true;

            Task.Factory.StartNew(() => startService()).ContinueWith(task => startServiceResult(task.Result), TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void startServiceResult(string ret)
        {
            if (ret == null)
            {
                picStartService.Image = statusImageList.Images[0];
                Application.DoEvents();
                done(true);
            }
            else
            {
                picStartService.Image = statusImageList.Images[1];
                MessageBox.Show(ret, "Start service error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                done(false);
            }
        }

        private string stopService()
        {
            if (!ExchangeHelper.isTransportServiceRunning())
            {
                return null;
            }

            try
            {
                ExchangeHelper.stopTransportService();
                return null;
            }
            catch (ExchangeHelperException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void stopServiceResult(string ret)
        {
            if (ret == null)
            {
                picStopService.Image = statusImageList.Images[0];
                Application.DoEvents();
                copyFilesTask();
            }
            else
            {
                picStopService.Image = statusImageList.Images[1];
                MessageBox.Show(ret, "Stop service error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                done(false);
            }
        }

        private void stopServiceTask()
        {
            lbStopService.Enabled = true;

            Task.Factory.StartNew(() => stopService()).ContinueWith(task => stopServiceResult(task.Result), TaskScheduler.FromCurrentSynchronizationContext());
        }

        private string directoryFromExchangeVersion()
        {
            string sExchangeVersion = null;

            Version oFullVersion = ExchangeHelper.getExchangeVersion();
            if (oFullVersion != null)
            {
                string sVersion = oFullVersion.ToString();

                if (sVersion.StartsWith("8.3.*"))
                    sExchangeVersion = "Exchange 2007 SP3";
                else if (sVersion.StartsWith("14.0."))
                    sExchangeVersion = "Exchange 2010";
                else if (sVersion.StartsWith("14.1."))
                    sExchangeVersion = "Exchange 2010 SP1";
                else if (sVersion.StartsWith("14.2."))
                    sExchangeVersion = "Exchange 2010 SP2";
                else if (sVersion.StartsWith("14.3."))
                    sExchangeVersion = "Exchange 2010 SP3";
                else if (sVersion.StartsWith("15.0.516.32"))
                    sExchangeVersion = "Exchange 2013";
                else if (sVersion.StartsWith("15.0.620.29"))
                    sExchangeVersion = "Exchange 2013 CU1";
                else if (sVersion.StartsWith("15.0.712.24"))
                    sExchangeVersion = "Exchange 2013 CU2";
                else if (sVersion.StartsWith("15.0.775.38"))
                    sExchangeVersion = "Exchange 2013 CU3";
                else if (sVersion.StartsWith("15.0.847.32"))
                    sExchangeVersion = "Exchange 2013 SP1 CU4";
                else if (sVersion.StartsWith("15.0.913.22"))
                    sExchangeVersion = "Exchange 2013 SP1 CU5";
            }

            return sExchangeVersion;
        }

        private void copyFilesTask()
        {
            lbCopyFiles.Enabled = true;

            Task.Factory.StartNew(() => copyFiles()).ContinueWith(task => copyFilesResult(task.Result), TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void copyFilesResult(string ret)
        {
            if (ret == null)
            {
                picCopyFiles.Image = statusImageList.Images[0];
                Application.DoEvents();
                installAgentTask();
            }
            else
            {
                picCopyFiles.Image = statusImageList.Images[1];
                MessageBox.Show(ret, "Copy error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                done(false);
            }
        }

        private string copyFiles()
        {
            // First copy the configuration executable from Src\Configuration.DkimSigner\bin\Release to the destination:
            try
            {
                string sourcePath = Path.Combine(tempPath, @"Src\Configuration.DkimSigner\bin\Release");
                string destPath = Path.Combine(installPath, "Configuration");
                string ret = copyAllFiles(sourcePath, destPath);
                if (ret != null)
                    return ret;

                //now copy the agent .dll from e.g. \Src\Exchange.DkimSigner\bin\Exchange 2007 SP3 to the destination
                //Get source directory for installed Exchange version:
                string libDir = directoryFromExchangeVersion();
                sourcePath = Path.Combine(tempPath, Path.Combine(@"Src\Exchange.DkimSigner\bin\", libDir));
                return copyAllFiles(sourcePath, installPath);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private string copyAllFiles(string sourceDir, string destDir)
        {
            string errorMsg = null;

            Action<String, String, Exception> copyComplete = (source, dest, e) =>
            {
                if (e != null)
                {
                    errorMsg = "Couldn't copy\n" + source + "\nto\n" + dest + "\n" + e.Message;
                }
            };

            foreach (string filename in Directory.EnumerateFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                string dest = Path.Combine(destDir, filename.Substring(sourceDir.Length + 1));
                string dir = Path.GetDirectoryName(dest);

                try
                {
                    Directory.CreateDirectory(dir);
                }
                catch (IOException ex)
                {
                    return "Couldn't create directory\n" + dir + "\n" + ex.Message;
                }

                FileHelper.CopyFile(filename, dest, copyComplete);

                if (errorMsg != null)
                    return errorMsg;
            }

            return null;
        }

        private void timUpgrade_Tick(object sender, EventArgs e)
        {
            timUpgrade.Enabled = false;
            startUpgrade();
        }

        /**********************************************************/
        /******************* Internal functions *******************/
        /**********************************************************/

        private void resetStatus()
        {
            this.lbStopService.Enabled = false;
            this.picStopService.Image = null;

            this.lbCopyFiles.Enabled = false;
            this.picCopyFiles.Image = null;

            this.lbInstallAgent.Enabled = false;
            this.picInstallAgent.Image = null;

            this.lbStartService.Enabled = false;
            this.picStartService.Image = null;

            this.lbDone.Enabled = false;
            this.picDone.Image = null;
        }

        private void done(bool success)
        {
            if (success)
            {
                this.lbDone.Enabled = true;
                picDone.Image = statusImageList.Images[0];
                dialogResult = DialogResult.OK;
            }
            else
            {
                dialogResult = DialogResult.Cancel;
            }

            btnClose.Enabled = true;
        }

        /**********************************************************/
        /********************** Button click **********************/
        /**********************************************************/

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = dialogResult;
            this.Close();
        }
    }
}
