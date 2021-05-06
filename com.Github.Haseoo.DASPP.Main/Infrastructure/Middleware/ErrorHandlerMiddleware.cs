﻿using com.Github.Haseoo.DASPP.CoreData.Dtos;
using com.Github.Haseoo.DASPP.CoreData.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace com.Github.Haseoo.DASPP.Main.Infrastructure.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
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
            catch (BadRequestException badRequestException)
            {
                await HandleException(context, HttpStatusCode.BadRequest, badRequestException);
            }
            catch (NotFoundException notFoundException)
            {
                await HandleException(context, HttpStatusCode.NotFound, notFoundException);
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse("Unhandled server error, contact system admin if error persists");
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            }
        }

        private static async Task HandleException(HttpContext context,
            HttpStatusCode statusCode,
            Exception exception)
        {
            var response = context.Response;
            response.StatusCode = (int)statusCode;
            response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(new ErrorResponse(exception));
            await response.WriteAsync(result);
        }
    }
}