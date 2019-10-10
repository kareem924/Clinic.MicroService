using System;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Common.General.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Common.Loggings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Common.RegisterContainers
{
    public static class HandlerRegister
    {
        public static void Register(Assembly assembly, IServiceCollection services)
        {
            services.AddMediatR(assembly);
        }

        public static IApplicationBuilder ConfigureAppBuilder(this IApplicationBuilder app, 
            ILoggerFactory loggerFactory, 
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            loggerFactory.AddProvider(new MicroservicesLoggerProvider(serviceProvider.GetService<IMessageBus>(), configuration));
            var logger = serviceProvider.GetService<ILogger>();
            app.UseErrorLogging(logger);
            return app;
           
        }
    }
}
