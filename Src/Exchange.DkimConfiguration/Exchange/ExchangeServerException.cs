using System;
using System.Runtime.Serialization;

namespace Exchange.DkimConfiguration.Exchange
{
    [Serializable]
    public class ExchangeServerException : Exception
    {
        public ExchangeServerException(string message) : base(message) { }

        public ExchangeServerException(string format, params object[] args) : base(string.Format(format, args)) { }

        public ExchangeServerException(string message, Exception innerException) : base(message, innerException) { }

        public ExchangeServerException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }

        protected ExchangeServerException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
