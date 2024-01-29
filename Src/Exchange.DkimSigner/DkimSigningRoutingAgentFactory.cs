using Exchange.DkimSigner.Configuration;
using Microsoft.Exchange.Data.Transport;
using Microsoft.Exchange.Data.Transport.Routing;
using System.IO;
using System.Reflection;

namespace Exchange.DkimSigner
{
	/// <summary>
	/// Creates new instances of the DkimSigningRoutingAgent.
	/// </summary>
	public sealed class DkimSigningRoutingAgentFactory : RoutingAgentFactory
	{
		/// <summary>
		/// The main DKIM Signer instance used for all emails
		/// </summary>
		private readonly DkimSigner dkimSigner;

		/// <summary>
		/// Watcher for changes on the settings file causing a reload of the settings when changed
		/// </summary>
		private FileSystemWatcher watcher;

		/// <summary>
		/// Initializes a new instance of the <see cref="DkimSigningRoutingAgentFactory"/> class.
		/// </summary>
		public DkimSigningRoutingAgentFactory()
		{
			if (Logger.IsDebugEnabled())
			{
				Logger.LogDebug("Initializing DkimSigner Service");
			}

			Settings config = new Settings();
			config.InitHeadersToSign();

			dkimSigner = new DkimSigner();

			LoadSettings();
			WatchSettings();
		}

		/// <summary>
		/// When overridden in a derived class, the 
		/// <see cref="M:Microsoft.Exchange.Data.Transport.Routing.RoutingAgentFactory.CreateAgent(Microsoft.Exchange.Data.Transport.SmtpServer)"/> 
		/// method returns an instance of a routing agent.
		/// </summary>
		/// <param name="server">The server on which the routing agent will operate.</param>
		/// <returns>The <see cref="DkimSigningRoutingAgent"/> instance.</returns>
		public override RoutingAgent CreateAgent(SmtpServer server)
		{
			return new DkimSigningRoutingAgent(dkimSigner);
		}

		/// <summary>
		/// Load the settings from the settings.xml file and apply the new settings to the dkimSigner instance.
		/// </summary>
		private void LoadSettings()
		{
			Settings config = new Settings();
			config.InitHeadersToSign();

			string assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			if (assemblyDir != null && config.Load(Path.Combine(assemblyDir, "settings.xml")))
			{
				dkimSigner.UpdateSettings(config);
				Logger.LogLevel = config.Loglevel;
				Logger.LogInformation("Exchange DKIM settings loaded: " + config.SigningAlgorithm + ", Canonicalization Header Algorithm: " + config.HeaderCanonicalization + ", Canonicalization Body Algorithm: " + config.BodyCanonicalization + ", Number of domains: " + dkimSigner.GetDomains().Count);
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
			watcher = new FileSystemWatcher
			{
				Path = Path.GetDirectoryName(filename),

				// Watch for changes in LastAccess and LastWrite times, and the renaming of files or directories.
				NotifyFilter = NotifyFilters.LastWrite,

				// Only watch text files.
				Filter = Path.GetFileName(filename)
			};

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