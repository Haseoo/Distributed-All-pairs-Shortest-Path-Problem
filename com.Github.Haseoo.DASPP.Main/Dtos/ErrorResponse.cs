using System;

namespace com.Github.Haseoo.DASPP.CoreData.Dtos
{
    public class ErrorResponse
    {
        public string Message { get; }
        public DateTime TimeStamp { get; }

        public ErrorResponse(string message)
        {
            Message = message;
            TimeStamp = DateTime.Now;
        }

        public ErrorResponse(Exception e)
        {
            Message = e.Message;
            TimeStamp = DateTime.Now;
        }
    }
}