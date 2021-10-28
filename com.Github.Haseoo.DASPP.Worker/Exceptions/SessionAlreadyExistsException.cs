using System;
using System.Runtime.Serialization;

namespace com.Github.Haseoo.DASPP.Main.Exceptions
{
    public class SessionAlreadyExistsException : Exception
    {
        public SessionAlreadyExistsException() : base("Session already exists")
        {
        }

        public SessionAlreadyExistsException(string message) : base(message)
        {
        }

        public SessionAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SessionAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}