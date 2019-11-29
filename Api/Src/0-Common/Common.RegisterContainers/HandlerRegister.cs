using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Common.Loggings;
using Common.RabbitMq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Security.API;

namespace Common.RegisterContainers
{
    public static class HandlerRegister
    {
        public static void Register(Assembly assembly, IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(assembly);
            services.AddCors(options =>
                options.
                    AddPolicy("AllowAll",
                        p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            services.AddIntegrationSupport();
            services.AddLogging();
            services.AddJwt(configuration);
            var concreteClasses = assembly.GetTypes()
                .Where(type =>
                    type.IsClass &&
                    !type.IsAbstract &&
                    !type.ContainsGenericParameters).ToArray();

            RegisterIntegrationEventHandler(concreteClasses, services);


        }

        public static IApplicationBuilder ConfigureAppBuilderExt(this IApplicationBuilder app,
            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            Assembly assembly)
        {
            app.AddIntegrationSupport(assembly);
            loggerFactory.AddProvider(new MicroservicesLoggerProvider(serviceProvider.GetService<IEventBus>(), configuration));
            var logger = serviceProvider.GetService<ILogger>();
            app.UseErrorLogging(logger);
            app.UseCors("AllowAll");
            return app;

        }

        private static void RegisterIntegrationEventHandler(Type[] classes, IServiceCollection services)
        {
            foreach (var implementationClass in classes)
            {
                var eventHandlerInterfaces = implementationClass.GetInterfaces()
                    .Where(@interface => @interface.IsGenericType &&
                                         @interface.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>))
                    .ToArray();
                foreach (var eventHandlerInterface in eventHandlerInterfaces)
                {
                    services.AddScoped(eventHandlerInterface, implementationClass);
                }
            }
        }
    }
}
