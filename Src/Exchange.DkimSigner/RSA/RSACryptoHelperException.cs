using System;
using System.Runtime.Serialization;

namespace DkimSigner.RSA
{
    [Serializable]
    public class RSACryptoHelperException : Exception
    {
        public RSACryptoHelperException(string message) : base(message) { }

        public RSACryptoHelperException(string format, params object[] args) : base(string.Format(format, args)) { }

        public RSACryptoHelperException(string message, Exception innerException) : base(message, innerException) { }

        public RSACryptoHelperException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }

        protected RSACryptoHelperException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
