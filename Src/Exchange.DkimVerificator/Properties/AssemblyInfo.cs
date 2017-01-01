using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Exchange DKIM Verificator")]
[assembly: AssemblyDescription("Verifies inbound e-mails using DKIM.")]
#if DEBUG
[assembly: AssemblyConfiguration("DEBUG")]
#else
[assembly: AssemblyConfiguration("RELEASE")]
#endif
[assembly: AssemblyCompany("github.com/Pro")]
[assembly: AssemblyProduct("Exchange DKIM Verificator")]
[assembly: AssemblyCopyright("MPL 2.0 by Stefan Profanter © 2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("a891a6ca-1060-453d-b730-661c91c56c89")]
[assembly: AssemblyVersion("4.0.0")]
[assembly: CLSCompliant(false)]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: AssemblyFileVersion("4.0.0")]