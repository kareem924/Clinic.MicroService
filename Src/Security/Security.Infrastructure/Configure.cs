using Common.Email;
using Common.General.UnitOfWork;
using Common.RegisterContainers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Security.Core.Repositories;
using Security.Infrastructure.Data;
using Security.Infrastructure.Data.Repositories;
using Security.Infrastructure.Service;
using System.Reflection;

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
            services.AddTransient<IEmailSender, EmailSenderService>();
            HandlerRegister.Register(Assembly.GetExecutingAssembly(), services);
        }

    }
}
