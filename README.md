Exchange DKIM Signer [![Build Status](https://travis-ci.org/Pro/dkim-exchange.png?branch=master)](https://travis-ci.org/Pro/dkim-exchange)&nbsp;[![Coverity Scan Build Status](https://scan.coverity.com/projects/3482/badge.svg)](https://scan.coverity.com/projects/3482)
=============

DKIM Signing Agent for Microsoft Exchange Server. This agent signs outgoing emails from your Exchange Server according to the DKIM specifications.

The code for the signing agent is based on the work from  http://nicholas.piasecki.name/blog/2010/12/dkim-signing-outbound-messages-in-exchange-server-2007/

We recommend to set up SPF (http://www.openspf.org) and DMARC (http://dmarc.org/) too. Test your email setup by sending an email to mailtest@unlocktheinbox.com (you will get an automatically generated report).

We are also happy for any donations to keep new versions flowing :) Especially if you think our DKIM signing agent helped you or your company preventing email spam.

<a href='https://pledgie.com/campaigns/28487'><img alt='Click here to lend your support to: DKIM Exchange Signer and make a donation at pledgie.com !' src='https://pledgie.com/campaigns/28487.png?skin_name=chrome' border='0' ></a>

## Supported versions

The DKIM Signer Agent [ExchangeDkimSigner.dll] is compiled for .NET 3.5 (Exchange 2007 and 2010) or .NET 4 (Exchange 2013)

* Exchange 2007 SP3 (8.3.*)
* Exchange 2010     (14.0.*)
* Exchange 2010 SP1 (14.1.*)
* Exchange 2010 SP2 (14.2.*)
* Exchange 2010 SP3 (14.3.*)
* Exchange 2013     (15.0.516.32)
* Exchange 2013 CU1 (15.0.620.29)
* Exchange 2013 CU2 (15.0.712.24)
* Exchange 2013 CU3 (15.0.775.38)
* Exchange 2013 SP1 (15.0.847.32)
* Exchange 2013 CU5 (15.0.913.22)
* Exchange 2013 CU6 (15.0.995.29)
* Exchange 2013 CU7 (15.0.1044.25)
* Exchange 2013 CU8 (15.0.1076.9)

## Requirements

* .NET 3.5 (Exchange 2007 or Exchange 2010) or .NET 4.0 (Exchange 2013)
* .NET 4.5 (optional - Configuration tool [Configuration.DkimSigner.exe])

Note : Manual install (see section below) is required if .NET 4.5 isn't installed

# Installing the Transport Agent

## Online install

1. Download the latest GUI package: https://github.com/Pro/dkim-exchange/releases/latest (Configuration.DkimSigner.zip)  
2. Extract it somewhere on your Server (e.g. Desktop)  
3. Start Configuration.DkimSigner.exe  
4. Select `Install`  
5. In the new opened window, select the version you like to install. If you want to install a prerelease version, check the corresponding box  
6. Press install and wait until the installation successfully finished, then close the window.  
7. Now configure the DKIM Signer with the installed GUI (located under `"C:\Program Files\Exchange DkimSigner\Configuration.DkimSigner.exe"`  
8. Once you save the config, the Signer Agent will automatically reload these changes  

## Offline Install

1. Download the latest GUI package: https://github.com/Pro/dkim-exchange/releases/latest (Configuration.DkimSigner.zip)  
2. Download the whole project package: https://github.com/Pro/dkim-exchange/releases/latest (Source Code (zip))
3. Move those two packages to your server and extract the `Configuration.DkimSigner.zip` package to your Desktop
4. Start Configuration.DkimSigner.exe
5. Select `Install`
6. In the new opened window, browse for the downloaded DkimSigner.zip and press `Install`
7. wait until the installation successfully finished, then close the window.
8. Now configure the DKIM Signer with the installed GUI (located under `"C:\Program Files\Exchange DkimSigner\Configuration.DkimSigner.exe"`
9. Once you save the config, the Signer Agent will automatically reload these changes

## Manual Install

If you have problems installing the agent using the options above, you can use the powershell script. Just follow these instructions:

1. Download the .zip and extract it e.g. on the Desktop: [Latest Release](https://github.com/Pro/dkim-exchange/releases/latest)
2. Open "Exchange Management Shell" from the Startmenu
3. Execute the following command to allow execution of local scripts (will be reset at last step): `Set-ExecutionPolicy Unrestricted`
4. Cd into the folder where the zip has been extracted.
5. Execute the install script `.\install.ps1`
6. Follow the instructions. For the configuration see next section.
7. Reset the execution policy: `Set-ExecutionPolicy Restricted`
8. Check EventLog for errors or warnings.
 Hint: you can create a user defined view in EventLog and then select "Per Source" and as the value "Exchange DkimSigner"

Make sure that the priority of the DkimSigner Agent is quite low so that no other agent messes around with the headers. Best set it to lowest priority.
To get a list of all the Export Agents use the Command `Get-TransportAgent`

To change the priority use `Set-TransportAgent -Identity "Exchange DkimSigner" -Priority 3`

## Problems?

If you have any problems installing, please check out the [troubleshooting guideline](https://github.com/Pro/dkim-exchange/blob/master/TROUBLESHOOT.md).  
**Exchange 2013 SP1**: If you have any problems installing the agent on Exchange 2013 SP1 please first try to apply the fix mentioned in issue [#24](https://github.com/Pro/dkim-exchange/issues/24)

# Configuring the agent

After installing the agent, you can use the Configuration.DkimSigner.exe within `C:\Program Files\Exchange DkimSigner` to configure the agent and all the settings. If the GUI doesn't work, you can also configure it manually (see section below).

## Configuration tool

<img alt='Information' src='https://github.com/Pro/dkim-exchange/blob/master/Screenshots/Information.png' border='0' >

<img alt='DKIM Settings' src='https://github.com/Pro/dkim-exchange/blob/master/Screenshots/DKIMSettings.png' border='0' >

<img alt='Domain Settings' src='https://github.com/Pro/dkim-exchange/blob/master/Screenshots/DomainSettings.png' border='0' >

<img alt='EventLogViewer' src='https://github.com/Pro/dkim-exchange/blob/master/Screenshots/EventLogViewer.png' border='0' >

<img alt='About' src='https://github.com/Pro/dkim-exchange/blob/master/Screenshots/About.png' border='0' >

## Manual configuration

Open `C:\Program Files\Exchange DkimSigner\settigs.xml` and configure the DKIM agent.

Here's an example file:

```xml
<?xml version="1.0" encoding="utf-8"?>
<Settings xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Loglevel>3</Loglevel>
  <SigningAlgorithm>RsaSha1</SigningAlgorithm>
  <HeaderCanonicalization>Relaxed</HeaderCanonicalization>
  <BodyCanonicalization>Relaxed</BodyCanonicalization>
  <HeadersToSign>
    <string>From</string>
    <string>Subject</string>
    <string>To</string>
    <string>Date</string>
    <string>Message-ID</string>
  </HeadersToSign>
  <Domains>
    <DomainElement>
      <Domain>example.com</Domain>
      <Selector>ex201302</Selector>
	  <!-- if relative path, then it's relative to C:\Program Files\Exchange DkimSigner\keys -->
      <PrivateKeyFile>example.com\ex201302.private</PrivateKeyFile>
    </DomainElement>
    <DomainElement>
      <Domain>example.org</Domain>
      <Selector>ex201302</Selector>
	  <!-- if relative path, then it's relative to C:\Program Files\Exchange DkimSigner\keys -->
      <PrivateKeyFile>example.org\ex201302.private</PrivateKeyFile>
    </DomainElement>
  </Domains>
</Settings>
```

You can add as many domain items as you need. For each domain item, the domain, the selector and the path to the private key file is needed.

This path may be relative or absolute.

Possible values for `HeaderCanonicalization` and `BodyCanonicalization` are `Simple` (recommended) and `Relaxed`.

### Logging
The dkim signing agent logs by default all errors and warnings into EventLog.
You can set the LogLevel in the `settings.xml` file:

Possible values:
* 0 = no logging
* 1 = Error only
* 2 = Warn+Error
* 3 = Info+Warn+Error
* 4 = Debug+Info+Warn+Error

The debug level should only be enabled if you need to debug functionality. Otherwise it will fill up your EventLog unnecessarily. Debug messages are shown with the information icon but will begin with the keyword 'DEBUG:'

## Creating the keys

You can create the private and public keys using Configuration.DkimSigner.exe (recommended) or you can create them with any other tool and then select them within the GUI.

You can use the following service for creating public and private keys:
http://www.port25.com/support/domainkeysdkim-wizard/

Or if you have a linux installation, use (from the opendkim package):
    opendkim-genkey -D target_directory/ -d example.com -s sel2012

The keys can be in DER or PEM format (the format will be automatically detected).
	
# Testing the setup

If you want to test, if everything is working, simply send a mail to mailtest@unlocktheinbox.com and you will get an immediate response with the results of the DKIM check.

# Updating the Transport Agent

If you want to update the Exchange DKIM Transport Agent simply run Configuration.DkimSigner.exe and on the `Information` tab press the Upgrade button. (If no new version is available the button shows 'Reinstall').

## Update from Version 1.* to 2.0.*

We had to change the settings file for the new version.
The best way to update is to make a backup copy from your `C:\Program Files\Exchange DkimSigner` directory. Then just follow the install and configure instructions. You can then also port the old configuration file (`ExchangeDkimSigner.dll.config`) from your backup by opening the new (`settings.xml`) and the old file in a text editor and manually editing the entries.

# Uninstalling the Transport Agent

If you want to uninstall the Exchange DKIM Transport Agent simply run Configuration.DkimSigner.exe and on the `Information` tab press the Configure button. In the new opened Window make sure the DKIM signer is selected. Then press `Uninstall`.

If you want to use the powershell script to uninstall the agent (not recommended) follow the manual install instructions but execute `.\uninstall.ps1` instead.

# Notes for developers

## Required DLLs for developing

For each Exchange Version we need the following files within the Lib directory:
<pre>
C:\Program Files\Microsoft\Exchange Server\V14\Public
Microsoft.Exchange.Data.Common.dll
Microsoft.Exchange.Data.Common.xml
Microsoft.Exchange.Data.Transport.dll
Microsoft.Exchange.Data.Transport.xml
</pre>

## Compiling

There are two projects in the Visual Studio Solution.
To compile the `Configuration.DKIMSigner` executable just go to Project Menu and then `Build Solution`.
To compile the .dll's for the Exchange Agent, got to Project Menu and then select  `Batch Build`. Make sure all the configurations are selected, then press Build. This will automatically link the agent DLLs with the correct version of the Exchange libraries.


## Debugging
If you want to debug the .dll on your Exchange Server, you need to install [Visual Studio Remote Debugging](http://msdn.microsoft.com/en-us/library/vstudio/bt727f1t.aspx) on the Server.

1. After the Remote Debugging Tools are installed on the Server, open Visual Studio
2. Compile the .dll with Debug information
3. Copy the recompiled .dll to the server
4. In Visual Studio select Debug->Attach to Process
5. Under 'Qualifier' input the server IP or Host Name
6. Select "Show processes from all users"
7. Select the process `EdgeTransport.exe` and then press 'Attach'
8. When reached, the process should stop at the breakpoint

# Changelog
* Current master branch:
	Fix : Check DNS TXT record (support multiline)
	Fix : Better GUI multithreading
	Fix : Improved install process
	Fix : Improved domain lookup in DKIM Signer
* 26.03.2015 [2.1.4]:  
	New: Support for Exchange 2013 SP1 CU8  
    Fix: Transparent background colors exception  
* 28.12.2014 [2.1.3]:  
    Fix: Update aborted with error that file can't be overwritten (#73)  
	New: Remember KeySize and show correct key size for existing keys  
	New: Support for Exchange 2013 SP1 CU7
* 27.11.2014 [2.1.2]:  
	Fix: update/install from GUI not working (ZIP extraction failed). Introduced in 2.1.0  
* 27.11.2014 [2.1.1]:  
	New: GUI shows now full changelog, including previous versions  
	Fix: Performance improvement for big attachments (#68)  
* 27.11.2014 [2.1.0]:  
	New: Support for PEM and DER encoded private keys  
	New: Added Debug log level  
	Fix: If subdomain and domain defined, uses wrong key (#67)  
	Fix: Other small bugfixes  
* 17.09.2014 [2.0.3]:  
	Fix: Wrong signature if subject contained colon and spaces. See issue #62  
* 11.09.2014 [2.0.2]:  
    New: The signer can now be configured through the GUI instead of manual XML editing  
    New: Installation, Update and Uninstall is now done by a simple click within GUI  
    New: Private key support (PEM, DER, XML)  
	New: Set agent priority through GUI  
	New: View Agent event log within GUI  
	Fix: Removed RecipientRule and SenderRule causing trouble when signing emails  
* 21.03.2014 [1.8.3]:  
	Fix: RecipientRule now matching whole address  
	Fix: Invalid E-Mail address doesn't cause a crash anymore. Rule will be ignored if error.  
	Fix: E-Mail address parsing in .NET 3.5 (Exchange 2007 & 2010)  
* 19.03.2014 [1.8.2]:  
	Fix: recipient rule not evaluated correctly (#26)
* 12.03.2014 [1.8.1]:  
	Fix: simple/simple signing fail introduced in version 1.7.0
* 12.03.2014 [1.8.0]:  
	New: 'Sender' config parameter
* 07.03.2014 [1.7.0]:  
	New: relaxed canonicalization (Thanks to @AlexLaroche)
* 04.02.2014 [1.6.0]:  
	New: `Rule` config parameter
* 18.01.2014 [1.5.2]:  
	Fix: message subject and body unicode encoding bug  
	New: support for Exchange 2013 CU1, CU2, CU3
* 27.11.2013 [1.5.1]:  
	New: support for Exchange 2013
* 26.11.2013 [1.5]:  
	Changed configuration file for better reading  
	New: compiled files for Exchange 2010 SP1&SP2  
	New: install and uninstall script
* 08.11.2013 [0.5]:  
    Changed build structure to do a batch build for different Exchange versions.  
	New: Build for Exchange 2010 and Exchange 2007.
* 24.02.2013:  
	New: multi domain support
