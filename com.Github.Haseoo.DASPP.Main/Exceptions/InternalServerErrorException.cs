using System;
using System.Runtime.Serialization;

namespace com.Github.Haseoo.DASPP.CoreData.Exceptions
{
    [Serializable]
    public class InternalServerErrorException : Exception
    {
        public InternalServerErrorException(string message) : base(message)
        {
        }

        protected InternalServerErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InternalServerErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InternalServerErrorException()
        {
        }
    }
}