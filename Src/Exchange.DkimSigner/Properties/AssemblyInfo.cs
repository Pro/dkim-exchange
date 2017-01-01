using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Exchange DKIM Signer")]
[assembly: AssemblyDescription("Signs outbound e-mails using DKIM.")]
#if DEBUG
[assembly: AssemblyConfiguration("DEBUG")]
#else
[assembly: AssemblyConfiguration("RELEASE")]
#endif
[assembly: AssemblyCompany("github.com/Pro")]
[assembly: AssemblyProduct("Exchange DKIM Signer")]
[assembly: AssemblyCopyright("MPL 2.0 by Stefan Profanter © 2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("FA0D7628-702A-445B-9C78-887874B212BA")]
[assembly: AssemblyVersion("4.0.0")]
[assembly: CLSCompliant(false)]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: AssemblyFileVersion("4.0.0")]
