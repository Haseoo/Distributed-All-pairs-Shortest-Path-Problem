using com.Github.Haseoo.DASPP.CoreData;
using com.Github.Haseoo.DASPP.CoreData.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace com.Github.Haseoo.DASPP.Main.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ProtectedByApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync
            (ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue
                (Constants.ApiKeyName, out var extractedApiKey))
            {
                context.Result = new ObjectResult(new ErrorResponse("Api Key was not provided"))
                {
                    StatusCode = 401
                };
                return;
            }
            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = configuration["ApiKey"];
            if (!apiKey.Equals(extractedApiKey))
            {
                context.Result = new ObjectResult(new ErrorResponse("Api Key is not Valid"))
                {
                    StatusCode = 401
                };
                return;
            }
            await next();
        }
    }
}