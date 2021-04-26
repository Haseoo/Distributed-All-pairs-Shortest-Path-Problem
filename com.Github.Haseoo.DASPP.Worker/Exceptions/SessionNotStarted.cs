using com.Github.Haseoo.DASPP.CoreData.Exceptions;
using System;
using System.Runtime.Serialization;

namespace com.Github.Haseoo.DASPP.Worker.Exceptions
{
    public class SessionNotStarted : NotFoundException
    {
        public SessionNotStarted() : base("Session has not been started")
        {
        }

        public SessionNotStarted(string message) : base(message)
        {
        }

        public SessionNotStarted(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SessionNotStarted(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}