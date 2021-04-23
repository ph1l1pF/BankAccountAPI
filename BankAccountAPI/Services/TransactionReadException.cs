using System;
using System.Runtime.Serialization;

namespace BankAccountAPI
{
    [Serializable]
    internal class TransactionReadException : Exception
    {
        public TransactionReadException()
        {
        }

        public TransactionReadException(string message) : base(message)
        {
        }

        public TransactionReadException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TransactionReadException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}