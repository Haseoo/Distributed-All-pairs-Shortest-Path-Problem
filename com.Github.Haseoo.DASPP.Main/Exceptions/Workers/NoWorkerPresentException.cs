using com.Github.Haseoo.DASPP.CoreData.Exceptions;
using System;
using System.Runtime.Serialization;

namespace com.Github.Haseoo.DASPP.Main.Exceptions.Workers
{
    [Serializable]
    public sealed class NoWorkerPresentException : BadRequestException
    {
        public NoWorkerPresentException() : base("Cannot perform request because no worker is registered!")
        {
        }

        private NoWorkerPresentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}