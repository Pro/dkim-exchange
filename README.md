dkim-exchange
=============

DKIM Signing Agent for Exchange Server.

This code is mainly based on the work of http://nicholas.piasecki.name/blog/2010/12/dkim-signing-outbound-messages-in-exchange-server-2007/

# Installing the Transport Agent

Copy thw whole content from the release of the project directory into a directory on the server, where Exchange runs.
Eg. into C:\Program Files\Exchange DKIM\

Then open Exchange Management Shell

	Install-TransportAgent -Name "Exchange DKIM" -TransportAgentFactory "Exchange.DkimSigner.DkimSigningRoutingAgentFactory" -AssemblyPath "C:\Program Files\Exchange DKIM\Exchange.DkimSigner.dll"
	 
	Enable-TransportAgent -Identity "Exchange DKIM"
	Restart-Service MSExchangeTransport

Interestingly, there will be a note telling you to close the Powershell window. It is not kidding. For some reason, the Install-TransportAgent cmdlet will keep a file handle open on our DLL, preventing Exchange from actually loading it until we close the Powershell window.

## Configuring the agent
Edit the .config file to fit your needs.


# Updating the Transport Agent

If you want to update the Exchange DKIM Transport Agent, you need to do the following:

* Open Powershell and stop the services, which block the .dll

        StopService MSExchangeTransport
        StopService W3SVC
       
* Then copy and overwrite the existing .dll
* Start the services again

        StartService W3SVC
        StartService MSExchangeTransport

# Notes for developers

## Required DLLs for developing

It isn't allowed to distribute the .dll required for development of this transport agent.
http://blogs.msdn.com/b/webdav_101/archive/2009/04/02/don-t-redistribute-product-dlls-unless-you-know-its-safe-and-legal-to-do-so.aspx

Therefore you have to copy all files from 
C:\Program Files\Microsoft\Exchange Server\V14\Public
Microsoft.Exchange.Data.Common.dll
Microsoft.Exchange.Data.Common.xml
Microsoft.Exchange.Data.Transport.dll
Microsoft.Exchange.Data.Transport.xml
into the Lib directory of this project.


