using Common.Email;
using Common.General.UnitOfWork;
using Common.RegisterContainers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Security.Infrastructure.Data;
using Security.Infrastructure.Data.Repositories;
using Security.Infrastructure.Service;
using System.Reflection;
using Common.General.Repository;
using Security.Core.Entities;
using Security.Core.Repositories;
using Security.Infrastructure.Interfaces;

namespace Security.Infrastructure
{
    public static class Configure
    {
        public static void ConfigureServices(IServiceCollection services, string connectionString, Assembly assembly)
        {
            //Context lifetime defaults to "scoped"
            services.AddDbContext<SecurityDbContext>(options => options.UseSqlServer(connectionString));
            
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IEmailSender, EmailSenderService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IJwtFactory, JwtFactory>();
            services.AddTransient<ITokenFactory, TokenFactory>();
            services.AddTransient<IJwtTokenHandler, JwtTokenHandler>();
            services.AddTransient<IJwtTokenValidator, JwtTokenValidator>();
            HandlerRegister.Register(assembly, services);
        }

    }
}
