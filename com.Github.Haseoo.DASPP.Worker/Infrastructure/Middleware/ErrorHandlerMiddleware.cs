using com.Github.Haseoo.DASPP.CoreData.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace com.Github.Haseoo.DASPP.Worker.Infrastructure.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (!(ex is FormatException) && !(ex is OverflowException))
                {
                    throw;
                }
                var response = context.Response;
                response.StatusCode = 449;
                response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new ErrorResponse(ex));
                await response.WriteAsync(result);
            }
        }
    }
}