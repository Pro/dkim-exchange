Exchange DKIM Signer [![Build Status](https://travis-ci.org/Pro/dkim-exchange.png?branch=master)](https://travis-ci.org/Pro/dkim-exchange)
=============

-----------------------------------------------------------------------------------------------------------------------

## Announcement

Stefan (@Pro) has kindly added me, Jonathan (@DJBenson) to the collaborators list so that I can help to keep this repository up to date. My changes have been merged into the master branch and I have created binary releases for v3.2.2 and v3.2.3 bringing support up to Exchange Server 2016 CU15 and Exchange Server 2019 CU4.

I will do my best to add new CU's as and when Microsoft release them, but will predominently be focussing on the latest version of Exchange (currently Exchange 2019) as I have that on-premise. I can and will add Exchange 2016 CU's but they will be untested by me.

-----------------------------------------------------------------------------------------------------------------------

## DKIM Signing Agent for Microsoft Exchange Server

This agent signs outgoing emails from your Exchange Server according to the DKIM specifications. It uses the DKIM signer implementation from the awesome [MimeKit](https://github.com/jstedfast/MimeKit) project.

We recommend to set up SPF (http://www.openspf.org) and DMARC (http://dmarc.org/) too. Test your email setup by sending an email to mailtest@unlocktheinbox.com (you will get an automatically generated report).

## Documentation

All documentation has now been migrated to the [Wiki](https://github.com/Pro/dkim-exchange/wiki) for ease of navigation and maintenance.

