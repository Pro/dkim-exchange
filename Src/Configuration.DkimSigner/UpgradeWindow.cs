using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Configuration.DkimSigner
{
    public partial class UpgradeWindow : Form
    {
        string tempPath;
        string installPath;
        DialogResult dialogResult;

        public UpgradeWindow(string tempPath, string installPath)
        {
            InitializeComponent();

            this.tempPath = tempPath;
            this.installPath = installPath;
            dialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void UpgradeWindow_Shown(object sender, EventArgs e)
        {
        }

        private void resetStatus()
        {
            lblStopService.Enabled = false;
            picStopService.Image = null;
            lblCopyFiles.Enabled = false;
            picCopyFiles.Image = null;
            lblInstallAgent.Enabled = false;
            picInstallAgent.Image = null;
            lblStartService.Enabled = false;
            picStartService.Image = null;
            lblDone.Enabled = false;
            picDone.Image = null;
        }

        private void startUpgrade()
        {
            resetStatus();
            stopServiceTask();
        }

        private string stopService()
        {
            if (!ExchangeHelper.isTransportServiceRunning())
                return null;
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
            lblStopService.Enabled = true;

            Task.Factory.StartNew(() => stopService()).ContinueWith(task => stopServiceResult(task.Result), TaskScheduler.FromCurrentSynchronizationContext());               
        }

        private void copyFilesTask()
        {
            lblCopyFiles.Enabled = true;

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
            string errorMsg = null;

            Action<String, String, Exception> copyComplete = (source, dest, e) =>
            {
                if (e != null)
                {
                    errorMsg = "Couldn't copy\n" + source + "\nto\n" + dest + "\n" + e.Message;
                }
            };

            foreach (string filename in Directory.EnumerateFiles(tempPath,"*", SearchOption.AllDirectories))
            {
                string dest = System.IO.Path.Combine(installPath, filename.Substring(tempPath.Length + 1));
                string dir = System.IO.Path.GetDirectoryName(dest);
                try
                {
                    System.IO.Directory.CreateDirectory(dir);
                }
                catch (IOException ex)
                {
                    return "Couldn't create directory\n" + dir + "\n"+ex.Message;
                }
                FileHelper.CopyFile(filename, dest, copyComplete);
                if (errorMsg!=null)
                    return errorMsg;
            }
            return null;
        }

        private void installAgentTask()
        {
            lblInstallAgent.Enabled = true;

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

        private string installAgent()
        {
            try
            {
                if (ExchangeHelper.isAgentInstalled())
                {
                    return null;
                }
                ExchangeHelper.installTransportAgent();
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
            lblStartService.Enabled = true;

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

        private void done(bool success)
        {
            if (success)
            {
                lblDone.Enabled = true;
                picDone.Image = statusImageList.Images[0];
                dialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                dialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            btnClose.Enabled = true;

        }

        private void timUpgrade_Tick(object sender, EventArgs e)
        {
            timUpgrade.Enabled = false;
            startUpgrade();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = dialogResult;
            this.Close();
        }
    }
}
