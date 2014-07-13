namespace Configuration.DkimSigner
{
    public class Constants
    {
        private Constants() { }

        public const string DKIM_SIGNER_PATH = @"C:\Program Files\Exchange DkimSigner";
        public const string DKIM_SIGNER_AGENT_NAME = @"Exchange DkimSigner";
        public const string DKIM_SIGNER_AGENT_DLL = @"ExchangeDkimSigner.dll";
        public const string DKIM_SIGNER_CONFIGURATION_EXE = @"Configuration.DkimSigner.exe";
    }
}
