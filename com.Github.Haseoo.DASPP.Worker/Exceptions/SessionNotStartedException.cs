using System;
using System.Runtime.Serialization;

namespace com.Github.Haseoo.DASPP.Worker.Exceptions
{
    public class SessionNotStartedException : Exception
    {
        public SessionNotStartedException() : base("Session has not been started")
        {
        }

        public SessionNotStartedException(string message) : base(message)
        {
        }

        public SessionNotStartedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SessionNotStartedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}