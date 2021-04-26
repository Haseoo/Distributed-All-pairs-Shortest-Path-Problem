using com.Github.Haseoo.DASPP.CoreData.Exceptions;
using System;
using System.Runtime.Serialization;

namespace com.Github.Haseoo.DASPP.Main.Exceptions.Workers
{
    [Serializable]
    public sealed class WorkerDoesNotExistException : NotFoundException
    {
        public WorkerDoesNotExistException(string uri) : base($"Worker with uri {uri} does not exist!")
        {
        }

        private WorkerDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}