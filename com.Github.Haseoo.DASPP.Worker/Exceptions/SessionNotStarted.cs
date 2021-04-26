using com.Github.Haseoo.DASPP.CoreData.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace com.Github.Haseoo.DASPP.Worker.Exceptions
{
    public class SessionNotStarted : NotFoundException
    {
        public SessionNotStarted()
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
