using System;
using System.Runtime.Serialization;

namespace Configuration.DkimSigner.Exchange
{
    [Serializable]
    public class ExchangeHelperException : Exception
    {
        public ExchangeHelperException(string message) : base(message) { }

        public ExchangeHelperException(string format, params object[] args) : base(string.Format(format, args)) { }

        public ExchangeHelperException(string message, Exception innerException) : base(message, innerException) { }

        public ExchangeHelperException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }

        protected ExchangeHelperException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
