using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Configuration.DkimSigner.Exchange;

namespace Configuration.DkimSigner
{
    public partial class InstallWindow : Form
    {
        /**********************************************************/
        /*********************** Variables ************************/
        /**********************************************************/

        private ExchangeServer oExchange = null;
        private bool bFailed = false;

        /**********************************************************/
        /*********************** Construtor ***********************/
        /**********************************************************/

        public InstallWindow(ExchangeServer oServer)
        {
            this.InitializeComponent();

            // Set label and picture
            //this.lbStopService.Enabled = false;
            //this.lbCopyFiles.Enabled = false;
            //this.lbInstallAgent.Enabled = false;
            //this.lbStartService.Enabled = false;

            this.picStopService.Image = null;
            this.picCopyFiles.Image = null;
            this.picInstallAgent.Image = null;
            this.picStartService.Image = null;

            // Load variable
            this.oExchange = oServer;
        }

        /**********************************************************/
        /************************* Events *************************/
        /**********************************************************/

        private void UpgradeWindow_Load(object sender, EventArgs e)
        {
            try
            {
                new Thread(new ThreadStart(this.InstallSafe)).Start();
            }
            catch (ThreadAbortException) { }
        }

        /**********************************************************/
        /******************* Internal functions *******************/
        /**********************************************************/

        private void InstallSafe()
        {
            //this.lbStopService.Enabled = true;
            //this.lbCopyFiles.Enabled = true;
            //this.lbInstallAgent.Enabled = true;
            //this.lbStartService.Enabled = true;

            //DownloadProgressWindow oDpw = new DownloadProgressWindow(dkimSignerAvailable.ZipballUrl, Path.GetTempPath());

            //if (oDpw.ShowDialog() == DialogResult.OK)
            //{
            //}

            // Stop Microsoft Exchange Transport Service
            this.StopService();

            if (!this.bFailed)
            {
                this.picStopService.Image = statusImageList.Images[0];
            }
            else
            {
                this.picStopService.Image = statusImageList.Images[1];
            }

            // Copy required files
            this.CopyFiles();

            if (!this.bFailed)
            {
                this.picCopyFiles.Image = statusImageList.Images[0];
            }
            else
            {
                this.picCopyFiles.Image = statusImageList.Images[1];
            }

            // Instal DKIM Signer agent in Microsoft Exchange Transport Service
            this.InstallAgent();

            if (!this.bFailed)
            {
                this.picInstallAgent.Image = statusImageList.Images[0];
            }
            else
            {
                this.picInstallAgent.Image = statusImageList.Images[1];
            }

            // Start Microsoft Exchange Transport Service
            this.StartService();

            if (!this.bFailed)
            {
                this.picStartService.Image = statusImageList.Images[0];
            }
            else
            {
                this.picStartService.Image = statusImageList.Images[1];
            }

            this.btnClose.Enabled = true;
        }

        private void StopService()
        {
            try
            {
                this.oExchange.StopTransportService();
            }
            catch (ExchangeHelperException) { }
        }

        private void CopyFiles()
        {
            // IF the directory "C:\Program Files\Exchange DkimSigner" doesn't exist, create it 
            if (!Directory.Exists(Constants.DKIM_SIGNER_PATH))
            {
                Directory.CreateDirectory(Constants.DKIM_SIGNER_PATH);
            }

            // First copy the configuration executable from Src\Configuration.DkimSigner\bin\Release to the destination:
            //string sourcePath = Path.Combine(sTempPath, @"Src\Configuration.DkimSigner\bin\Release");
            //string destPath = Path.Combine(sInstallPath, "Configuration");
            //string ret = copyAllFiles(sourcePath, destPath);

            //now copy the agent .dll from e.g. \Src\Exchange.DkimSigner\bin\Exchange 2007 SP3 to the destination
            //Get source directory for installed Exchange version:
            //string libDir = directoryFromExchangeVersion();
            //sourcePath = Path.Combine(sTempPath, Path.Combine(@"Src\Exchange.DkimSigner\bin\", libDir));
            //return copyAllFiles(sourcePath, sInstallPath);
        }

        /*private string copyAllFiles(string sourceDir, string destDir)
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

                FileHelper.CopyFile(filename, dest, copyComplete);

                if (errorMsg != null)
                    return errorMsg;
            }

            return null;
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
        }*/

        /// <summary>
        /// 
        /// </summary>
        private void InstallAgent()
        {
            if (!this.oExchange.IsTransportServiceInstalled())
            {

                // First make sure the following Registry key exists
                // HKLM:\SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM
                if (RegistryHelper.Open(@"SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM") != null)
                {
                    RegistryHelper.WriteSubKeyTree(@"SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM");
                }

                // Install DKIM Transport Agent in Microsoft Exchange 
                try
                {
                    this.oExchange.InstallDkimTransportAgent();
                }
                catch (ExchangeHelperException) { }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void StartService()
        {
            try
            {
                this.oExchange.StartTransportService();
            }
            catch (ExchangeHelperException) { }
        }

        /**********************************************************/
        /********************** Button click **********************/
        /**********************************************************/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}