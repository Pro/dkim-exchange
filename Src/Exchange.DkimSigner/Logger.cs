using System.Diagnostics;

namespace Exchange.Dkim
{
    public class Logger
    {
        /// <summary>
        /// The log level defined.
        /// Set to initial default level to log all
        /// </summary>
        public static int LogLevel = 99;

        private static EventLog logger;

        public static void LogDebug(string message)
        {
            LogDebug(message, 0);
        }

        public static void LogDebug(string message, int id)
        {
            if (LogLevel >= 4)
            {
                LogEntry("DEBUG: " + message, id, EventLogEntryType.Information);
            }
        }

        public static void LogInformation(string message)
        {
            LogInformation(message, 0);
        }

        public static void LogInformation(string message, int id)
        {
            if (LogLevel >= 3)
            {
                LogEntry(message, id, EventLogEntryType.Information);
            }
        }

        public static void LogWarning(string message)
        {
            LogWarning(message, 0);
        }

        public static void LogWarning(string message, int id)
        {
            if (LogLevel >= 2)
            {
                LogEntry(message, id, EventLogEntryType.Warning);
            }
        }

        public static void LogError(string message)
        {
            LogError(message, 0);
        }

        public static void LogError(string message, int id)
        {
            if (LogLevel >= 1)
            {
                LogEntry(message, id, EventLogEntryType.Error);
            }
        }

        private static void LogEntry(string message, int id, EventLogEntryType logType)
        {
            if (logger == null)
            {
                logger = new EventLog();
                logger.Source = "Exchange DKIM";
            }

            logger.WriteEntry(message, logType, id);
        }
    }
}