using Common.General.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Security.Core.Interfaces;
using Security.Core.Repositories;
using Security.Core.Services;
using Security.Infrastructure.Data;
using Security.Infrastructure.Data.Repositories;
using Security.Infrastructure.Interfaces;
using Security.Infrastructure.Service;

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
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IJwtFactory, JwtFactory>();
            services.AddTransient<IJwtTokenHandler, JwtTokenHandler>();
            services.AddTransient<IJwtTokenValidator, JwtTokenValidator>();
            services.AddTransient<ITokenFactory, TokenFactory>();

        }

    }
}
