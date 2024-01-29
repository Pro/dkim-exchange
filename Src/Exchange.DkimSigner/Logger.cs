using System.Diagnostics;

namespace Exchange.DkimSigner
{
	public static class Logger
	{
		/// <summary>
		/// The log level defined.
		/// Set to initial default level to log all
		/// </summary>
		public static int LogLevel { get; set; } = 99;

		private static EventLog logger;

		public static void LogDebug(string message)
		{
			if (LogLevel >= 4)
			{
				LogEntry("DEBUG: " + message, 0, EventLogEntryType.Information);
			}
		}

		public static bool IsDebugEnabled()
		{
			return LogLevel >= 4;
		}

		public static void LogInformation(string message)
		{
			if (LogLevel >= 3)
			{
				LogEntry(message, 0, EventLogEntryType.Information);
			}
		}

		public static void LogWarning(string message)
		{
			if (LogLevel >= 2)
			{
				LogEntry(message, 0, EventLogEntryType.Warning);
			}
		}

		public static void LogError(string message)
		{
			if (LogLevel >= 1)
			{
				LogEntry(message, 0, EventLogEntryType.Error);
			}
		}

		private static void LogEntry(string message, int id, EventLogEntryType logType)
		{
			if (logger == null)
			{
				logger = new EventLog
				{
					Source = "Exchange DKIM"
				};
			}

			logger.WriteEntry(message, logType, id);
		}
	}
}