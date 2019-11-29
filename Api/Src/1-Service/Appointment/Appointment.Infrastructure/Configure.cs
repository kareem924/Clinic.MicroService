using Microsoft.Extensions.DependencyInjection;
using Appointment.Core.Entities;
using Appointment.Infrastructure.Data.Repositories;
using Common.General.Repository;
using System.Reflection;
using Common.RegisterContainers;
using Microsoft.Extensions.Configuration;

namespace Appointment.Infrastructure
{
    public static class Configure
    {
        public static void ConfigureServices(IServiceCollection services,IConfiguration configuration)
        {
            services.AddTransient<IRepository<Session>, SessionRepository>();
            services.AddTransient<IRepository<Core.Entities.Appointment>, AppointmentRepository>();
            HandlerRegister.Register(Assembly.GetExecutingAssembly(), services, configuration);
        }
    }
}
