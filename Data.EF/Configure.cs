using Auth.Infrastructure.Data.UnitOfWork;
using Clinic.SharedKernel.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infrastructure
{
    public class Configure
    {   //Context lifetime defaults to "scoped"

        public static void ConfigureServices(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ClinicDbContext>(options => options.UseSqlServer(connectionString));
            services.AddTransient<IUnitOfWork, Uow>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
        }

    }
}
