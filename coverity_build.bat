REM ------------------------------------------------------------------------------------
REM script executing the necessary commands to create a coverity build output directory.
REM You need to download the coverity tools: https://scan.coverity.com/download?tab=csharp
REM and add them to your PATH environment variable.
REM Use a Visual Studio Developer Command Prompt to execute this script (msbuild is required)
REM After executing this script, there's a new folder called 'cov-int'.
REM You need to ZIP it and upload it to coverity manually.
REM ------------------------------------------------------------------------------------

SET mypath=%~dp0
cd %mypath%
rmdir cov-int /s /q
cov-build --dir cov-int msbuild /t:Build coverity.proj