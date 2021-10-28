using com.Github.Haseoo.DASPP.CoreData.Dtos;
using com.Github.Haseoo.DASPP.CoreData.Exceptions;
using com.Github.Haseoo.DASPP.Main.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace com.Github.Haseoo.DASPP.Main.Infrastructure.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next,
            ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                ErrorResponse errorResponse = null;
                switch (ex)
                {
                    case SessionAlreadyExistsException _:
                    case SessionNotStartedException _:
                        response.StatusCode = 449;
                        break;

                    case NotFoundException _:
                        response.StatusCode = StatusCodes.Status404NotFound;
                        break;

                    default:
                        response.StatusCode = StatusCodes.Status500InternalServerError;
                        errorResponse = new ErrorResponse("Unhandled server error, contact system admin if error persists");
                        _logger.LogError(ex.Message);
                        _logger.LogError(ex.StackTrace);
                        break;
                }
                var result = JsonSerializer.Serialize(errorResponse ?? new ErrorResponse(ex));
                await response.WriteAsync(result);
            }
        }
    }
}