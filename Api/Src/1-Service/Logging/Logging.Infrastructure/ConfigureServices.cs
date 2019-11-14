using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Common.General.Repository;
using Common.RegisterContainers;
using Logging.Core.Entities;
using Logging.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Logging.Infrastructure
{
    public static class Configure
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IRepository<LogEntry>, LoggingRepository>();

            HandlerRegister.Register(Assembly.GetExecutingAssembly(), services);
        }
    }
}
