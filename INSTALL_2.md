This file documents on how to install Exchange DKIM Version 2.0
=============

# Installing the agent

## Online install

1) Download the latest GUI package: https://github.com/Pro/dkim-exchange/releases/download/v2.0.0-beta/Configuration.DkimSigner.zip
2) Extract it somewhere on your Server (e.g. Desktop)
3) Start Configuration.DkimSigner.exe
4) If you want to install a prerelease version, check the corresponding box
5) Press install and wait until the installation successfully finished, then close the window.
6) Now configure the DKIM Signer with the installed GUI (located under `"C:\Program Files\Exchange DkimSigner\Configuration\Configuration.DkimSigner.exe"`
7) Once you save the config, the Signer Agent will automatically reload these changes

## Offline Install

1) Download the latest GUI package: https://github.com/Pro/dkim-exchange/releases/download/v2.0.0-beta/Configuration.DkimSigner.zip
2) Download the whole project package: https://github.com/Pro/dkim-exchange/archive/v2.0.0-beta.zip
3) Move those two packages to your server and extract the `Configuration.DkimSigner.zip` package to your Desktop
4) Start Configuration.DkimSigner.exe
5) Select `Install from .zip` and select the whole project package downloaded on step 2
6) wait until the installation successfully finished, then close the window.
7) Now configure the DKIM Signer with the installed GUI (located under `"C:\Program Files\Exchange DkimSigner\Configuration\Configuration.DkimSigner.exe"`
8) Once you save the config, the Signer Agent will automatically reload these changes