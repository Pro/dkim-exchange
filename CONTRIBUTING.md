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

### Creating Releases

1. Make sure that the new version number is set correctly in the Visual Studio Project AssemblyInfo (Right click project -> Settings -> Assembly Info). This needs to be updated for both projects, the configurator and the lib
2. Optionally update the Copyright year
3. Update the CHANGELOG.md
4. Build all the .dll versions and the Configurator as described in the Compiling Section
5. Commit the changed files, including resulting binary files (`Src/Exchange.DkimSigner/bin`, and `Src/Configuration.DkimSigner/bin/Release`)
6. Push all the changes
7. Go to GitHub -> Releases (https://github.com/Pro/dkim-exchange/releases)
8. Create a new release. As the tag use the same version you used in step 1, e.g. `v3.2.0`. The title should summarize the release. And as text use the change log content. Just refer to previous releases.
9. Create a new .zip File for the binary:
   Right-click on `Src/Configuration.DkimSigner/bin/Release` and then zip the folder. Rename the resulting .zip to `Configuration.DkimSigner.zip`. Make sure that this .zip does not contain any subfolders, but all the files directly.
10. Add the .zip to the release
11. Create the release as a pre-release. This will allow other users to test it as a beta version
12. After two weeks or so just remove the pre-release flag, and it will be picked up by the configurator.exe as a new update

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
