using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Runtime.CompilerServices;
using System.Text;

namespace Configuration.DkimSigner.Exchange
{
    public class PowerShellHelper
    {
        private static Runspace runspace = null;

        public static readonly string[] EXCHANGE_PSSNAPIN =
        {
            "Microsoft.Exchange.Management.PowerShell.Admin",   // Exchange 2007
            "Microsoft.Exchange.Management.PowerShell.E2010",   // Exchange 2010 
            "Microsoft.Exchange.Management.PowerShell.SnapIn"   // Exchange 2013
        };

        /// <summary>
        /// Execute a specific command within the PowerShell and return its output.
        /// Only one command at a time can be executed
        /// </summary>
        /// <param name="sCommand">The command to execute</param>
        /// <param name="bRemoveEmptyLines">Remove empty lines from output</param>
        /// <returns>The output of the command as string.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static string ExecPowerShellCommand(string sCommand, bool bRemoveEmptyLines)
        {
            if (PowerShellHelper.runspace == null)
            {
                PSSnapInInfo info = null;
                PSSnapInException ex = null;
                bool error = false;

                try
                {
                    PowerShellHelper.runspace = RunspaceFactory.CreateRunspace(RunspaceConfiguration.Create());
                    PowerShellHelper.runspace.Open();

                    foreach (string pssnapin in PowerShellHelper.EXCHANGE_PSSNAPIN)
                    {
                        if (PowerShellHelper.Execute("$(Get-PSSnapin -Registered | Select-String " + pssnapin + ") -ne $null", true).Trim() == "True")
                        {
                            info = PowerShellHelper.runspace.RunspaceConfiguration.AddPSSnapIn(pssnapin, out ex);
                        }
                    }
                }
                catch (Exception)
                {
                    error = true;
                }

                if (ex != null || info == null || error)
                {
                    PowerShellHelper.runspace.Dispose();
                    PowerShellHelper.runspace = null;
                    throw new ExchangeServerException("Couldn't initialize PowerShell runspace.");
                }
            }

            return PowerShellHelper.Execute(sCommand, bRemoveEmptyLines);
        }

        private static string Execute(string sCommand, bool bRemoveEmptyLines)
        {
            StringBuilder sb = new StringBuilder();
            Pipeline pipeline = null;
            Exception except = null;

            try
            {
                pipeline = PowerShellHelper.runspace.CreatePipeline();
                pipeline.Commands.Clear();
                pipeline.Commands.AddScript(sCommand);
                pipeline.Commands.Add("Out-String");
                Collection<PSObject> collection = pipeline.Invoke();

                foreach (PSObject current in collection)
                {
                    sb.AppendLine(current.ToString());
                }

                if (bRemoveEmptyLines)
                {
                    string[] array = sb.ToString().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    sb = new StringBuilder();

                    for (int i = 0; i < array.Length; i++)
                    {
                        string value = array[i].Trim();

                        if (!string.IsNullOrEmpty(value))
                        {
                            sb.AppendLine(value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                except = ex;
            }
            finally
            {
                if (pipeline != null)
                {
                    pipeline.Dispose();
                }
            }

            if (except != null)
            {
                throw except;
            }

            return sb.ToString();
        }
    }
}
