using Common.Commands;
using Common.Events;
using Common.Extensions.System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace Common.RegisterContainers
{
    public static class HandlerRegister
    {
        public static void Register(Assembly assembly, IServiceCollection services)
        {

            services.AddScoped<ICommandBus, CommandBus>();
            services.AddScoped<IEventBus, EventBus>();

            var allCommandHandler = assembly.GetTypes().Where(t =>
                t.IsClass &&
                !t.IsAbstract &&
                t.IsAssignableToGenericType(typeof(ICommandHandler<>)));
            foreach (var type in allCommandHandler)
            {
                var allInterfaces = type.GetInterfaces();
                var mainInterfaces = allInterfaces.Where(t => t.IsAssignableToGenericType(typeof(ICommandHandler<>)));
                foreach (var itype in mainInterfaces)
                {
                    services.AddScoped(itype, type);
                }
            }

            var allEventHandlers = assembly.GetTypes().Where(t =>
               t.IsClass &&
               !t.IsAbstract &&
               t.IsAssignableToGenericType(typeof(IEventHandler<>)));
            foreach (var type in allEventHandlers)
            {
                var allInterfaces = type.GetInterfaces();
                var mainInterfaces = allInterfaces.Where(t => t.IsAssignableToGenericType(typeof(IEventHandler<>)));
                foreach (var itype in mainInterfaces)
                {
                    services.AddScoped(itype, type);
                }
            }
        }
    }
}
