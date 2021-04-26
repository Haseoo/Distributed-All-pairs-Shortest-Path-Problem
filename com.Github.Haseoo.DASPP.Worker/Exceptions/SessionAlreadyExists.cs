using com.Github.Haseoo.DASPP.CoreData.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace com.Github.Haseoo.DASPP.Worker.Exceptions
{
    public class SessionAlreadyExists : BadRequestException
    {
        public SessionAlreadyExists() : base("Session alredy exists")
        {
        }

        public SessionAlreadyExists(string message) : base(message)
        {
        }

        public SessionAlreadyExists(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SessionAlreadyExists(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
