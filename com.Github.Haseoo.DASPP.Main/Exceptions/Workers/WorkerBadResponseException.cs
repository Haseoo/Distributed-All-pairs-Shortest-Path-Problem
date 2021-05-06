using com.Github.Haseoo.DASPP.CoreData.Dtos;
using com.Github.Haseoo.DASPP.CoreData.Exceptions;
using System;
using System.Runtime.Serialization;
using System.Text.Json;

namespace com.Github.Haseoo.DASPP.Main.Exceptions.Workers
{
    [Serializable]
    public sealed class WorkerBadResponseException : InternalServerErrorException
    {
        public WorkerBadResponseException(int code, string errorMessage) :
            base($"Worker responded with code ${code} and message ${JsonSerializer.Deserialize<ErrorResponse>(errorMessage).Message}.")
        {
        }

        private WorkerBadResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}