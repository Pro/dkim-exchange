------------------------------------------------------------------------------
CONFIGURE THE AGENT
------------------------------------------------------------------------------

Edit the .config file as appropriate, configuring Log4Net as necessary and
replacing the <appSettings> values as appropriate for the server. In the
default configuration, it only logs messages if something blows up, and the
log file appears in the AppData folder of the user that the "Microsoft
Exchange Transport" service is running as. On an SBS 2008 box, this
corresponds to the

C:\Windows\ServiceProfiles\NetworkService\AppData\Roaming\Skiviez\Wolverine\DkimSigner\Logs

folder.

------------------------------------------------------------------------------
INSTALL THE AGENT
------------------------------------------------------------------------------

From the Exchange Management Shell,

Install-TransportAgent -Name "DKIM Signer for Exchange" -TransportAgentFactory "Skiviez.Wolverine.Exchange.DkimSigner.DkimSigningRoutingAgentFactory" -AssemblyPath "C:\Program Files\Skiviez\Wolverine\DKIM Signer for Exchange\Skiviez.Wolverine.Exchange.DkimSigner.dll"

Enable-TransportAgent -Name "DKIM Signer for Exchange"

Then exit the shell and restart the "Microsoft Exchange Transport" service. 
Some users with Outlook open may note delays for several minutes after 
restarting the transport service; you can mitigating this by restarting the 
"Microsoft Exchange Mail Submission" service after restarting the
"Microsoft Exchange Transport" service.