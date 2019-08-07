using System;
using System.Collections.Generic;
using System.Text;
using Common.General.Repository;
using Common.General.Specification;
using Common.General.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Security.Core.Entities;
using Security.Core.Interfaces;
using Security.Core.Repositories;
using Security.Infrastructure.Data;
using Security.Infrastructure.Data.Repositories;
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
            //services.AddTransient(), typeof(EfRepository<>));
            //services.AddTransient(typeof(ISpecification<>), typeof(BaseSpecification<>));
            services.AddTransient<IJwtFactory, JwtFactory>();
            services.AddTransient<ITokenFactory, TokenFactory>();

        }

    }
}
