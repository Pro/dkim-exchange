using System.IO;
using System.Reflection;
using Exchange.DkimSigner.Configuration;

namespace Exchange.Dkim
{
    class SettingsWatcher
    {
        /// <summary>
        /// Watcher for changes on the settings file causing a reload of the settings when changed
        /// </summary>
        private FileSystemWatcher watcher;

        /// <summary>
        /// Class where updateSettings is called when the settings change
        /// </summary>
        private SettingsReceiver settingsReceiver;

        /// <summary>
        /// Constructor for settings watcher.
        /// </summary>
        /// <param name="receiver">UpdateSettings is called on the receiver when the settings change.</param>
        public SettingsWatcher(SettingsReceiver receiver)
        {
            this.settingsReceiver = receiver;
        }

        /// <summary>
        /// Load the settings from the settings.xml file and apply the new settings to the dkimSigner instance.
        /// </summary>
        public void LoadSettings()
        {
            Settings config = new Settings();
            config.InitHeadersToSign();

            string assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (assemblyDir != null && config.Load(Path.Combine(assemblyDir, "settings.xml")))
            {
                Logger.LogLevel = config.Loglevel;
                settingsReceiver.UpdateSettings(config);
            }
            else
            {
                Logger.LogError("Couldn't load the settings file.\n");
            }
        }

        /// <summary>
        /// Watch for changes in the settings.xml file and reload them if a change is detected
        /// </summary>
        public void WatchSettings()
        {
            string assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (assemblyDir == null)
            {
                Logger.LogWarning("Could not get directory name from path: " + Assembly.GetExecutingAssembly().Location + "\nSettings watcher disabled.");
                return;
            }
            string filename = Path.Combine(assemblyDir, "settings.xml");

            // Create a new FileSystemWatcher and set its properties.
            watcher = new FileSystemWatcher();
            watcher.Path = Path.GetDirectoryName(filename);

            // Watch for changes in LastAccess and LastWrite times, and the renaming of files or directories.
            watcher.NotifyFilter = NotifyFilters.LastWrite;

            // Only watch text files.
            watcher.Filter = Path.GetFileName(filename);

            // Add event handlers.
            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;

            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Event handler for handling change detection of the file watcher for settings.xml.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            Logger.LogInformation("Detected settings file change. Reloading...");
            LoadSettings();
        }
    }
}
