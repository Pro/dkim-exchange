namespace Configuration.DkimSigner.Exchange
{
    public class TransportServiceAgent
    {
        public string Name { get; set; }
		public bool Enabled { get; set; }
		public int Priority  { get; set; }

		public TransportServiceAgent() {}

        public TransportServiceAgent(string sName, bool bEnabled, int iPriority)
		{
			this.Name = sName;
			this.Enabled = bEnabled;
			this.Priority = iPriority;
		}
    }
}
