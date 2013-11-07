dkim-exchange
=============

DKIM Signing Agent for Exchange Server.

This code is mainly based on the work of http://nicholas.piasecki.name/blog/2010/12/dkim-signing-outbound-messages-in-exchange-server-2007/

WARNING: Please read the 'Known Bugs' section before you continue!

## Supported versions

This Transport Agent is fully tested under Exchange 2010 SP3 with Windows Server 2008 R2.

If it's running on other version not mentioned here, please notify me, so I can update it here.

Exchange 2010 SP2 doesn't seem to be supported. See Issue #5. If you have SP2 installed, please read Issue #5 and send me the requested files so that I can recompile it for SP2, then it should work.

Exchange 2007 SP3 .dll is build and can be found in the release directory. Please check if those are working for you and send me a short notice.

## Installing the Transport Agent

1. Copy the .dll mathing your Exchange Server version from the [release directory](Release) into a directory on the server, where Exchange runs.
Eg. into `C:\Program Files\Exchange DKIM\`. Also copy the `Exchange.DkimSigner.dll.config` to the same directory. The final structure should be:
<pre>
C:\Program Files\Exchange DKIM\Exchange.DkimSigner.dll
C:\Program Files\Exchange DKIM\Exchange.DkimSigner.dll.config
</pre>

2. Create the registry key for EventLog by executing the script: [Create Key.reg](Utils/Create key.reg?raw=true)

4. Add `C:\Program Files\Exchange DKIM\` to your PATH environment variable:

 Normal command prompt: `set "path=%path%;C:\Program Files\Exchange DKIM"`
 
 or in the Power shell: `setx PATH "$env:path;C:\Program Files\Exchange DKIM" -m`

 (If you execute the following command in the same shell, you need to first restart the shell load the new environment vaiable)

5. Then open Exchange Management Shell
<pre>
	Install-TransportAgent -Name "Exchange DKIM" -TransportAgentFactory "Exchange.DkimSigner.DkimSigningRoutingAgentFactory" -AssemblyPath "C:\Program Files\Exchange DKIM\Exchange.DkimSigner.dll"
	 
	Enable-TransportAgent -Identity "Exchange DKIM"
	Restart-Service MSExchangeTransport
</pre>
6. Close the Exchange Management Shell Window
7. Check EventLog for errors or warnings

### Configuring the agent
Edit the .config file to fit your needs.

```xml
<domainInfo>
  <domain Domain="example.com" Selector="sel2012" PrivateKeyFile="keys/example.com.private"/>
  <domain Domain="example.org" Selector="sel2013" PrivateKeyFile="keys/example.org.private"/>
</domainInfo>
```

You can add as many domain items as you need. For each domain item, the domain, the selector and the path to the private key file is needed.
This path may be relative (based on the location of the .dll) or absolute.

#### Logging
The dkim signing agent logs by default all errors and warnings into EventLog.
You can set the LogLevel in the .config file:

```xml
<setting name="LogLevel" serializeAs="String">
  <value>2</value>
</setting> 
```

Possible values:
0 = no logging
1 = Error only
2 = Warn+Error
3 = Info+Warn+Error

### Creating the keys

You can use the following service for creating public and private keys:
http://www.port25.com/support/domainkeysdkim-wizard/

Or if you have a linux installation, use (from the opendkim package):
    opendkim-genkey -D target_directory/ -d example.com -s sel2012

### Testing the setup

If you want to test, if everything is working, simply send a mail to check-auth@verifier.port25.com and you will get an immediate response with the results of the DKIM check.

## Updating the Transport Agent

If you want to update the Exchange DKIM Transport Agent, you need to do the following:

* Open Powershell and stop the services, which block the .dll

        StopService MSExchangeTransport
       
* Then download [Exchange.DkimSigner.dll](Release/) and overwrite the existing .dll
* Start the services again

        StartService MSExchangeTransport

## Known bugs

* [unconfirmed] When using internal Receive Connectors as Relay, unicode characters may break and will be replaced with '?'. See: Issue #2

## Notes for developers

### Required DLLs for developing

It isn't allowed to distribute the .dll required for development of this transport agent.
http://blogs.msdn.com/b/webdav_101/archive/2009/04/02/don-t-redistribute-product-dlls-unless-you-know-its-safe-and-legal-to-do-so.aspx

Therefore you have to copy all files from 
<pre>
C:\Program Files\Microsoft\Exchange Server\V14\Public
Microsoft.Exchange.Data.Common.dll
Microsoft.Exchange.Data.Common.xml
Microsoft.Exchange.Data.Transport.dll
Microsoft.Exchange.Data.Transport.xml
</pre>
into the Lib directory of this project.

## Changelog

* 24.02.2013:
	Added multi domain support

## TODO

* Allow use of relaxed or any combination between relaxed and simple
