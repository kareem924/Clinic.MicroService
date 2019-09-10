using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace Common.Loggings
{
    public static class MiddlewareExtensions
    {
        public static void UseErrorLogging<T>(this IApplicationBuilder builder, ILogger<T> logger)
        {
            builder.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogError(contextFeature.Error, $"Something went wrong: {contextFeature.Error}");

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            context.Response.StatusCode,
                            contextFeature.Error.Message
                        }));
                    }
                });
            });
        }
    }
}
