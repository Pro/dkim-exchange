namespace Configuration.DkimSigner
{
    public class Constants
    {
        private Constants() { }

        public const string DKIM_SIGNER_PATH = @"C:\Program Files\Exchange DkimSigner";
        public const string DKIM_SIGNER_AGENT_NAME = @"Exchange DkimSigner";
        public const string DKIM_SIGNER_AGENT_DLL = @"ExchangeDkimSigner.dll";
        public const string DKIM_SIGNER_CONFIGURATION_EXE = @"Configuration.DkimSigner.exe";

        public const string DKIM_SIGNER_VERSION = "Version 2.0.3";
        public const string DKIM_SIGNER_NOTICE = "This is free software; see the source for copying conditions. There is NO warranty; not even for MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.";
        public const string DKIM_SIGNER_LICENCE = "This Software is provided under the GNU GPLv3 License.\r\nFuther details can be found here: http://www.gnu.org/licenses";
        public const string DKIM_SIGNER_AUTHOR = "The code for the signing algorithm is based on the work of Nicholas Piasecki.\r\nhttp://nicholas.piasecki.name/blog/2010/12/dkim-signing-outbound-messages-in-exchange-server-2007/\r\n\r\nDeveloped and maintained by Stefan Profanter and Alexandre Laroche.";
        public const string DKIM_SIGNER_WEBSITE = "Website : https://github.com/Pro/dkim-exchange";

        public const string DKIM_SIGNER_EVENTLOG_SOURCE = @"Exchange DKIM";
    }
}