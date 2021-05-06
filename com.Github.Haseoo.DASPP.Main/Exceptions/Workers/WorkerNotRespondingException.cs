using com.Github.Haseoo.DASPP.CoreData.Exceptions;
using System;
using System.Runtime.Serialization;

namespace com.Github.Haseoo.DASPP.Main.Exceptions.Workers
{
    [Serializable]
    public sealed class WorkerNotRespondingException : InternalServerErrorException
    {
        public WorkerNotRespondingException(string uri) :
            base($"Worker {uri} not responding.")
        {
        }

        private WorkerNotRespondingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}