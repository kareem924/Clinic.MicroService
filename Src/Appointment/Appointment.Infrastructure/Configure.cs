using Appointment.Infrastructure.Data;
using Appointment.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appointment.Infrastructure
{
    public static class Configure
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IAppointmentDbContext, AppointmentDbContext>();

        }

    }
}
