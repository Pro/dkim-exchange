dkim-exchange
=============

DKIM Signing Agent for Exchange Server

# Installing the Transport Agent

Copy thw whole content from the release of the project directory into a directory on the server, where Exchange runs.
Eg. into C:\Program Files\Exchange DKIM\

Then open Exchange Management Shell

	Install-TransportAgent -Name "Exchange DKIM" -TransportAgentFactory "Exchange.DkimSigner.DkimSigningRoutingAgentFactory" -AssemblyPath "C:\Program Files\Exchange DKIM\Exchange.DkimSigner.dll"
	 
	Enable-TransportAgent -Identity "Exchange DKIM"

Interestingly, there will be a note telling you to close the Powershell window. It is not kidding. For some reason, the Install-TransportAgent cmdlet will keep a file handle open on our DLL, preventing Exchange from actually loading it until we close the Powershell window.

To make it actually work, we need to restart the Microsoft Exchange Transport service.

## Configuring the agent
Edit the .config file to fit your needs.


# Notes for developer

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


