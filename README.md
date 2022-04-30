# Exchange DKIM Signer

## About

Exchange DKIM Signer adds support to Microsoft Exchange Server for applying a [DomainKeys Identified Mail](https://en.wikipedia.org/wiki/DomainKeys_Identified_Mail) signature to outgoing messages.

DKIM is an email authentication method that can help detect forged sender addresses in email, a technique often used in phishing and email spam. It's often used along with other email authentication methods such as SPF and DMARC.

Exchange DKIM Signer is 'clean' - it doesn't contain any advertising or send any telemetry. The configuration tool includes an online check (to GitHub) to see whether a new version is available, and has a download and install feature.

## Requirements

Exchange DKIM Signer is compatible with the following versions of Microsoft Exchange Server:
* Exchange Server 2019 (RTM version and later)
* Exchange Server 2016 (CU13 and later)
* Exchange Server 2013 (CU23 and later)

If you require support for earlier versions of Exchange, use [Exchange DKIM Signer 3.3.4](https://github.com/Pro/dkim-exchange/releases/tag/v3.3.4).

Currently, Windows Server Core is not recommended when used with Exchange Server 2019 as Exchange DKIM Signer uses a GUI configuration tool.

## DKIM Signing Agent for Microsoft Exchange Server

The Agent signs outgoing emails from your Exchange server according to the DKIM specifications. It uses the DKIM signing implementation from the [MimeKit](http://www.mimekit.net/) project.

We recommend to set up SPF (http://www.open-spf.org/) and DMARC (https://dmarc.org/) too. Test your email configuration by using a service such as [DKIM Test](https://www.appmaildev.com/en/dkim).

## Documentation

Full documentation is available in the [Wiki](https://github.com/Pro/dkim-exchange/wiki).