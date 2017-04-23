Exchange DKIM Signer [![Build Status](https://travis-ci.org/Pro/dkim-exchange.png?branch=master)](https://travis-ci.org/Pro/dkim-exchange)&nbsp;[![Coverity Scan Build Status](https://scan.coverity.com/projects/3482/badge.svg)](https://scan.coverity.com/projects/3482)
=============

**Developer/Maintainer wanted!**

**Unfortunately I don't have time to maintain and implement all the features I would like to. If you would like to help developing within this project contact me by creating an issue.
Thanks!!**


DKIM Signing Agent for Microsoft Exchange Server. This agent signs outgoing emails from your Exchange Server according to the DKIM specifications. It uses the DKIM signer implementation from the awesome [MimeKit](https://github.com/jstedfast/MimeKit) project.

We recommend to set up SPF (http://www.openspf.org) and DMARC (http://dmarc.org/) too. Test your email setup by sending an email to mailtest@unlocktheinbox.com (you will get an automatically generated report).

We are also happy for any donations to keep new versions flowing :) Especially if you think our DKIM signing agent helped you or your company preventing email spam.

<a href='https://pledgie.com/campaigns/28487'><img alt='Click here to lend your support to: DKIM Exchange Signer and make a donation at pledgie.com !' src='https://pledgie.com/campaigns/28487.png?skin_name=chrome' border='0' ></a>

## Supported versions

The DKIM Signer Agent [ExchangeDkimSigner.dll] is compiled for .NET 3.5 (Exchange 2007 and 2010) or .NET 4 (Exchange 2013 & 2016)

* Exchange 2007 SP3     (8.3.*)
* Exchange 2010         (14.0.*)
* Exchange 2010 SP1     (14.1.*)
* Exchange 2010 SP2     (14.2.*)
* Exchange 2010 SP3     (14.3.*)
* Exchange 2013         (15.0.516.32)
* Exchange 2013 CU1     (15.0.620.29)
* Exchange 2013 CU2     (15.0.712.24)
* Exchange 2013 CU3     (15.0.775.38)
* Exchange 2013 SP1     (15.0.847.32)
* Exchange 2013 CU5     (15.0.913.22)
* Exchange 2013 CU6     (15.0.995.29)
* Exchange 2013 CU7     (15.0.1044.25)
* Exchange 2013 CU8     (15.0.1076.9)
* Exchange 2013 CU9     (15.0.1104.5)
* Exchange 2013 CU10    (15.0.1130.7)
* Exchange 2013 CU11    (15.0.1156.6)
* Exchange 2013 CU12    (15.0.1178.4)
* Exchange 2013 CU13    (15.0.1210.3)
* Exchange 2013 CU14    (15.0.1236.3)
* Exchange 2013 CU15    (15.0.1263.5)
* Exchange 2016 Preview (15.1.225.17)
* Exchange 2016 RTM     (15.1.225.42)
* Exchange 2016 CU1     (15.1.396.30)
* Exchange 2016 CU2 	(15.1.466.34)
* Exchange 2016 CU3		(15.1.544.27)
* Exchange 2016 CU4		(15.1.669.32)
* Exchange 2016 CU5		(15.1.845.34)

## Requirements

* .NET 3.5 (Exchange 2007 or Exchange 2010) or .NET 4.0 (Exchange 2013, 2016)
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
3. Check your current execution policy and remember it for the last step `Get-ExecutionPolicy –List`
4. Execute the following command to allow execution of local scripts (will be reset at last step): `Set-ExecutionPolicy Unrestricted`
5. Cd into the folder where the zip has been extracted.
6. Execute the install script `.\install.ps1`
7. Follow the instructions. For the configuration see next section.
8. Reset the execution policy to the value from step 3. E.g., `Set-ExecutionPolicy RemoteSigned` or `Set-ExecutionPolicy Restricted`
9. Check EventLog for errors or warnings.
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
    `opendkim-genkey -D target_directory/ -d example.com -s sel2012`  
or openssl:  
    `openssl genrsa -out private.pem`  

The keys have to be in PEM format.

# Testing the setup

If you want to test, if everything is working, simply send a mail to mailtest@unlocktheinbox.com and you will get an immediate response with the results of the DKIM check.

# Updating the Transport Agent

If you want to update the Exchange DKIM Transport Agent simply run Configuration.DkimSigner.exe and on the `Information` tab press the Upgrade button. (If no new version is available the button shows 'Reinstall').

## Updating from version 2.* to 3.*

If you are updating the agent from version 2.* to 3.* you may need to recreate your private key and update the DNS record correspondingly if you used the .xml key format.
We now switched to another key library which doesn't support the .xml format anymore.

To recreate your private key just open the Configuration tool and for each domain press the `Generate key` button. Make sure to update the DNS too.

Additionally, the `Message-ID` header field changed its name. Open the `DKIM Settings` Tab in the configurator, remove the `Message-ID` entry from `Headers to sign` list, and add `MessageId` as the corresponding header.

# Updating Exchange Server

Before you update the Exchange Server, you have to make sure that the DKIM Signer Version is compatible with the new Exchange Version. Thus the following steps are suggested to avoid any Upgrade problems:

1. Disable the DKIM Signer (Open the configuration executable, on the `Information` tab press `Configure`, then disable the DKIM Signer)
2. Update the Exchange Server
3. Update the DKIM Signer (using the configuration executable)
4. Re-enable the DKIM Signer

# Uninstalling the Transport Agent

If you want to uninstall the Exchange DKIM Transport Agent simply open the Control Panel and then `Programs and Features`. The DKIM Signer should be listed there. You can also start Configuration.DkimSigner.exe with the `--uninstall` parameter, which does the same. Be aware that uninstalling the agent also removes all the keys and files.

If you want to use the powershell script to uninstall the agent (not recommended) follow the manual install instructions but execute `.\uninstall.ps1` instead.

# Notes for developers

See [contribution guideline](https://github.com/Pro/dkim-exchange/blob/master/CONTRIBUTING.md)
