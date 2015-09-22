// <copyright file="AssemblyInfo.cs" company="Skiviez, Inc."> 
// Copyright (c) 2010 by Skiviez, Inc. All rights reserved. 
// </copyright>

using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("DKIM Signer for Exchange")]
[assembly: AssemblyDescription("Signs outbound e-mails using DKIM.")]
#if DEBUG
[assembly: AssemblyConfiguration("DEBUG")]
#else
[assembly: AssemblyConfiguration("RELEASE")]
#endif
[assembly: AssemblyCompany("github.com/Pro")]
[assembly: AssemblyProduct("Exchange DKIM")]
[assembly: AssemblyCopyright("Gnu GPL 3 by Stefan Profanter © 2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("FA0D7628-702A-445B-9C78-887874B212BA")]
[assembly: AssemblyVersion("2.1.7")]
[assembly: CLSCompliant(true)]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: AssemblyFileVersionAttribute("2.1.7")]
