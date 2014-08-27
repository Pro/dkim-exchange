using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;

namespace Configuration.DkimSigner.Exchange
{
    public class PowerShellHelper
	{
        private static Runspace runspace = null;

        /// <summary>
        /// Ensure singleton runspace creation.
        /// </summary>
        /// <returns>A singleton instance of a runspace which supports Exchange PowerShell Management commands.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private static Runspace getRunspace()
        {
            if (runspace != null)
                return runspace;
            RunspaceConfiguration runspaceConfiguration = RunspaceConfiguration.Create();
	        PSSnapInException ex = null;
	        PSSnapInInfo pSSnapInInfo = null;

            // Exchange 2007
	        try
	        {
		        pSSnapInInfo = runspaceConfiguration.AddPSSnapIn("Microsoft.Exchange.Management.PowerShell.Admin", out ex);
	        }
	        catch {}

            // Exchange 2010 
	        try
	        {
		        pSSnapInInfo = runspaceConfiguration.AddPSSnapIn("Microsoft.Exchange.Management.PowerShell.E2010", out ex);
	        }
	        catch {}

            // Exchange 2013
    	    try
	        {
		        pSSnapInInfo = runspaceConfiguration.AddPSSnapIn("Microsoft.Exchange.Management.PowerShell.SnapIn", out ex);
	        }
	        catch {}
    
            if (pSSnapInInfo != null)
            {
                Exception except = null;

                try
                {
                    runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
                    runspace.Open();
                }
                catch (Exception exc)
                {
                    except = exc;
                    runspace.Dispose();
                    runspace = null;
                }
                if (except != null)
                    throw except;
            }
            else
            {
                throw new Configuration.DkimSigner.Exchange.ExchangeHelperException("Couldn't initialize PowerShell Runspace");
            }

            return runspace;
        }

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
            Runspace runspace = getRunspace();

            Pipeline pipeline = null;
            Exception except = null;

            string result = null;
            try
            {
                pipeline = runspace.CreatePipeline();
                pipeline.Commands.Clear();
                pipeline.Commands.AddScript(sCommand);
                pipeline.Commands.Add("Out-String");
                Collection<PSObject> collection = pipeline.Invoke();

                StringBuilder stringBuilder = new StringBuilder();

                foreach (PSObject current in collection)
                {
                    stringBuilder.AppendLine(current.ToString());
                }

                string text = stringBuilder.ToString();
                if (bRemoveEmptyLines)
                {
                    stringBuilder = new StringBuilder();
                    string[] array = text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < array.Length; i++)
                    {
                        string value = array[i].Trim();

                        if (!string.IsNullOrEmpty(value))
                        {
                            stringBuilder.AppendLine(value);
                        }
                    }

                    text = stringBuilder.ToString();
                }

                result = text;
            }
            catch (Exception exc)
            {
                except = exc;
                runspace = null;
            }
            finally
            {
                pipeline.Dispose();
            }
            if (except != null)
                throw except;
            return result;
        }
    }

}