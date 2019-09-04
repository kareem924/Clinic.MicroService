using Microsoft.Extensions.DependencyInjection;
using Appointment.Core.Entities;
using Appointment.Infrastructure.Data.Repositories;
using Common.General.Repository;

namespace Appointment.Infrastructure
{
    public static class Configure
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IRepository<Session>, SessionRepository>();
            services.AddTransient<IRepository<Core.Entities.Appointment>, AppointmentRepository>();
        }
    }
}
