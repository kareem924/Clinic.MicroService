using Common.General.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Security.Core.Interfaces;
using Security.Core.Repositories;
using Security.Infrastructure.Data;
using Security.Infrastructure.Data.Repositories;

namespace Security.Infrastructure
{
    public static class Configure
    {
        public static void ConfigureServices(IServiceCollection services, string connectionString)
        {
            //Context lifetime defaults to "scoped"
            services.AddDbContext<SecurityDbContext>(options => options.UseSqlServer(connectionString));
            
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUserRepository, UserRepository>();
           

        }

    }
}
