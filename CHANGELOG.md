# Changelog

## Current master branch:  



## Releases
* 23.08.2018 [3.1.1]:
	New: Added support for Exchange Exchange 2013 CU21
	New: Added support for Exchange 2016 CU10
* 31.05.2018 [3.1.0]:  
	New: Added support for Exchange Exchange 2013 CU20  
	New: Added support for Exchange 2016 CU9  
	Fix: Ignore Unknown header in header to sign list (#177)  
	Fix: Update through GUI not working due to SSL issue (#224)  
* 12.10.2017 [3.0.12]:  
	New: Added support Exchange 2016 CU6
	New: Added support Exchange 2016 CU7
* 12.10.2017 [3.0.11]:  
	New: Support for Exchange 2013 SP1 CU17
	New: Support for Exchange 2013 SP1 CU18
* 24.05.2017 [3.0.10]:  
	New: Support for Exchange 2013 SP1 CU16
* 23.04.2017 [3.0.9]:  
	New: Support for Exchange 2016 CU5  
	Fix: Sort list of domains in Configuration tool  
* 22.12.2016 [3.0.8]:  
	New: Support for 2007 SP3 RU21 & 2013 SP1 CU15 & 2016 CU4  
* 26.10.2016 [3.0.7]:  
	New: Support for Exchange 2013 CU14  
* 25.09.2016 [3.0.6]:  
	New: Support for Exchange 2016 CU3  
* 09.09.2016 [3.0.5]:  
	New: Support for Exchange 2013 SP1 CU13  
* 13.07.2016 [3.0.4]:  
	New: Support for Exchange 2016 CU2  
* 29.03.2016 [3.0.3]:  
	New: Support for Exchange 2013 SP1 CU12 and 2016 CU1  
* 27.02.2016 [3.0.2]:  
	Fix: Updated to MimeKit 1.2.21 (Fix to not enforce 7bit encoding of the body: https://github.com/jstedfast/MimeKit/issues/224)  
	Fix: Now using system variable for Program Files Folder install location instead of hard-coded `C:\Program Files`  
* 17.01.2016 [3.0.1]:  
	New: Switched to MimeKit implementation for DKIM signer  
	New: Better handling of private key reading and generation. Now using OpenSSL format 
	New: The configurator now checks if the public key in the DNS record matches the suggested value 
    New: The Control Panel now contains a link to the DKIM signer configurator  
	New: The DKIM signer can now be uninstalled using `Program and Features` from the control panel  
	New: Support for Exchange 2013 SP1 CU11  
    Fix: Various GUI improvements and smaller fixes  
	Fix: Agent 'Exchange DkimSigner' went async (#112)  
    Fix: GUI issues when running Exchange 2007 (#104)
* 30.10.2015 [2.1.8]:  
	New: Support for Exchange 2016 RTM  
	Fix: Performance improvement by creating DKIM signer only once (#103 and #95)  
	Fix: Empty FromAddress (e.g. in NDR) is now handled correctly (#99)  
	Fix: Fixed install/upgrade taking wrong .exe after download (#102)  
* 22.09.2015 [2.1.7]:  
	New: Support for Exchange 2013 SP1 CU10  
* 11.09.2015 [2.1.6]:  
    Fix: Event Log Message `did not call Resume on the new thread` (#97)  
* 21.08.2015 [2.1.5]:  
    New: Support for Exchange 2016 Preview   
    New: Support for Exchange 2013 SP1 CU9  
    New: `--debug` command line parameter for debug tab  
	Fix: Check DNS TXT record (support multiline)  
    Fix: Better GUI multithreading  
    Fix: Improved install process  
    Fix: Improved domain lookup in DKIM Signer  
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

