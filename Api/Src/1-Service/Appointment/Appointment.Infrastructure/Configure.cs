using Microsoft.Extensions.DependencyInjection;
using Appointment.Core.Entities;
using Appointment.Infrastructure.Data.Repositories;
using Common.General.Repository;
using System.Reflection;
using Common.RegisterContainers;

namespace Appointment.Infrastructure
{
    public static class Configure
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IRepository<Session>, SessionRepository>();
            services.AddTransient<IRepository<Core.Entities.Appointment>, AppointmentRepository>();
            HandlerRegister.Register(Assembly.GetExecutingAssembly(), services);
        }
    }
}
