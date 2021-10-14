using System;

namespace Configuration.DkimSigner
{
	public static class Constants
	{
		public static string DkimSignerPath
		{
			get { return string.Format("{0}\\Exchange DkimSigner", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).TrimEnd('\\')); }
		}

		public const string DkimSignerAgentName = @"Exchange DkimSigner";
		public const string DkimSignerAgentDll = @"ExchangeDkimSigner.dll";
		public const string DkimSignerConfigurationExe = @"Configuration.DkimSigner.exe";

		public const string DkimSignerWebsite = "https://github.com/Pro/dkim-exchange";

		public const string DkimSignerEventlogSource = @"Exchange DKIM";
		public const string DkimSignerEventlogRegistry = @"SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM";

		internal static string GetShortenedVersionString(string version) => GetShortenedVersionString(Version.Parse(version));
		internal static string GetShortenedVersionString(Version version) => $"{version.Major}.{version.Minor}.{version.Build}";
	}
}