# Contributing to dkim-exchange

:tada::+1: First of all, thank you for contributing! :+1::tada:

This document contains guidelines and helpful information for contributing to the dkim-exchange project.

#### Table Of Contents

[Notes for developers](#nodes-for-developers)
  * [How to include support for newer Exchange Versions](#how-to-include-support-for-newer-exchange-versions)
  * [Compiling](#compiling)
  * [Debugging](#debugging)


## Notes for developers

### How to include support for newer Exchange Versions

If you want to help us supporting new Exchange Versions, please follow the steps below.

For each Exchange Version we need the following files within the Lib directory:
<pre>
C:\Program Files\Microsoft\Exchange Server\V14\Public (or the corresponding directory for your version)
Microsoft.Exchange.Data.Common.dll
Microsoft.Exchange.Data.Common.xml
Microsoft.Exchange.Data.Transport.dll
Microsoft.Exchange.Data.Transport.xml
</pre>

1. Create a fork of our project in your account
2. Create a new branch for your changes
3. Add the above mentioned files (.dll and .xml) to your fork. Create a new directory for the corresponding Exchange version in the [Lib](https://github.com/Pro/dkim-exchange/tree/master/Lib) directory.
4. If you are not familiar with Visual Studio or coding in C# please stop here and just create a new pull request. Otherwise continue with the following steps.
5. Now you need to change each of the following files to include the new .dll. Refer e.g. to this commit: [f83924a](https://github.com/Pro/dkim-exchange/commit/f83924a3b9fef6c0dfd1b85526f6182207bac55b)
 * DkimSigner.sln
 * README.md
 * Src/Configuration.DkimSigner/Constants.cs
 * Src/Exchange.DkimSigner/Exchange.DkimSigner.csproj
 * coverity.proj
 * install.ps1
6. Make sure that everything compiles. See [Compiling](#compiling)
7. Create a pull request


### Compiling

There are two projects in the Visual Studio Solution.
To compile the `Configuration.DKIMSigner` executable just go to Project Menu and then `Build Solution`.
To compile the .dll's for the Exchange Agent, go to Project Menu and then select  `Batch Build`. Make sure all the configurations are selected, then press Build. This will automatically link the agent DLLs with the correct version of the Exchange libraries.


### Debugging
If you want to debug the .dll on your Exchange Server, you need to install [Visual Studio Remote Debugging](http://msdn.microsoft.com/en-us/library/vstudio/bt727f1t.aspx) on the Server.

1. After the Remote Debugging Tools are installed on the Server, open Visual Studio
2. Compile the .dll with Debug information
3. Copy the recompiled .dll to the server
4. In Visual Studio select Debug->Attach to Process
5. Under 'Qualifier' input the server IP or Host Name
6. Select "Show processes from all users"
7. Select the process `EdgeTransport.exe` and then press 'Attach'
8. When reached, the process should stop at the breakpoint
