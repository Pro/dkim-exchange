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

            int parIdx = Math.Max(Math.Max(Array.IndexOf(asArgv, "--install"), Array.IndexOf(asArgv, "--upgrade")), Array.IndexOf(asArgv, "--configure"));
            if (parIdx >= 0)
            {
                if (asArgv[parIdx] == "--configure")
                {
                    oForm = new ConfigureWindow();
                }
                else if (asArgv[parIdx] == "--install" || asArgv[parIdx] == "--upgrade")
                {
                    string installZipUrl = null;
                    if (asArgv.Length > parIdx + 1)
                    {
                        installZipUrl = asArgv[parIdx + 1];
                    }

                    oForm = new InstallWindow(installZipUrl);
                }
                //else if (asArgv[parIdx] == "--uninstall") { }
                else
                {
                    Application.Exit();
                }
            }
            else
            {
                int debugIdx = Array.IndexOf(asArgv, "--debug");
                oForm = new MainWindow(debugIdx >= 0); 
            }

            Application.Run(oForm);
        }
    }
}