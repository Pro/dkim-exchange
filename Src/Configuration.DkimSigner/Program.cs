using System;
using System.Windows.Forms;

using Configuration.DkimSigner.Exchange;

namespace Configuration.DkimSigner
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // *******************************************************************
            // Set some default application configuration
            // *******************************************************************
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // *******************************************************************
            // Load correct Windows form
            // *******************************************************************
            Form oForm = null;
            string[] asArgv = Environment.GetCommandLineArgs();
            
            int parIdx = Math.Max(Array.IndexOf(asArgv, "--install"), Array.IndexOf(asArgv, "--upgrade"));
            if (parIdx >= 0)
            {
                if (asArgv[parIdx] == "--install") {}
                else if (asArgv[parIdx] == "--upgrade") {}
                //else if (asArgv[parIdx] == "--uninstall") { }
                else
                {
                    Application.Exit();
                }

                string installZipUrl = null;
                if (asArgv.Length > parIdx + 1)
                {
                    installZipUrl = asArgv[parIdx + 1];
                }

                oForm = new InstallWindow(installZipUrl);
            }
            else
            {
                oForm = new MainWindow(); 
            }

            Application.Run(oForm);
        }
    }
}