using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;

namespace Configuration.DkimSigner.Exchange
{
    public class PowerShellHelper
	{
        public static string ExecPowerShellCommand(string sCommand, bool bRemoveEmptyLines)
        {
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

    
	        string result = null;
	        if (pSSnapInInfo != null)
	        {
		        Runspace runspace = null;
		        Pipeline pipeline = null;
		        
                try
		        {
			        runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
			        runspace.Open();
			        pipeline = runspace.CreatePipeline();
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
				        string[] array = text.Split(new char[]{'\n'}, StringSplitOptions.RemoveEmptyEntries);
				       
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
		        finally
		        {
			        pipeline.Dispose();
			        runspace.Dispose();
		        }
	        }

	        return result;
        }
    }
}