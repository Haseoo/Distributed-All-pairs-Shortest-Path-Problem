using System;
using System.Runtime.Serialization;

namespace com.Github.Haseoo.DASPP.Main.Exceptions.Workers
{
    [Serializable]
    public sealed class WorkerAlreadyRegisteredException : BadRequestException
    {
        public WorkerAlreadyRegisteredException(string uri) : base($"Worker with uri {uri} is already registered!")
        {
        }

        private WorkerAlreadyRegisteredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}