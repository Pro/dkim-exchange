using System;
using System.Windows.Forms;

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

            /*while (!System.Diagnostics.Debugger.IsAttached)
                System.Threading.Thread.Sleep(100);*/

            // *******************************************************************
            // Load correct Windows form
            // *******************************************************************
            Form oForm = null;
            string[] asArgv = Environment.GetCommandLineArgs();



            int parIdx = Math.Max(Math.Max(Array.IndexOf(asArgv, "--install"),
                Array.IndexOf(asArgv, "--upgrade-inplace")),
                Array.IndexOf(asArgv, "--configure"));

            if (parIdx >= 0)
            {
                if (asArgv[parIdx] == "--configure")
                {
                    oForm = new ConfigureWindow();
                }
                else if (asArgv[parIdx] == "--upgrade-inplace")
                {
                    oForm = new InstallWindow(true);
                }
                else if (asArgv[parIdx] == "--install")
                {
                    oForm = new InstallWindow();
                }
            }
            else
            {
                if (Array.IndexOf(asArgv, "--register") > 0)
                {
                    CplControl.Register();
                }
                else if (Array.IndexOf(asArgv, "--unregister") > 0)
                {
                    CplControl.Unregister();
                }
                else
                {
                    int debugIdx = Array.IndexOf(asArgv, "--debug");
                    oForm = new MainWindow(debugIdx >= 0);
                }
            }

            if (oForm != null)
            {
                Application.Run(oForm);
            }
        }
    }
}