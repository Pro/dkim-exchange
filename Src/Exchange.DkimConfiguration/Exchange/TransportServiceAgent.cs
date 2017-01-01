namespace Exchange.DkimConfiguration.Exchange
{
    public class TransportServiceAgent
    {
        public string Name { get; set; }
		public bool Enabled { get; set; }
		public int Priority  { get; set; }

		public TransportServiceAgent() {}

        public TransportServiceAgent(string sName, bool bEnabled, int iPriority)
		{
			Name = sName;
			Enabled = bEnabled;
			Priority = iPriority;
		}
    }
}
