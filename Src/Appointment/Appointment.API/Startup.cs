using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Common.General.Dto;
using Common.General.Interfaces;
using Common.General.SharedKernel;
using Common.Loggings;
using Common.MongoDb;
using Common.RabbitMq;
using Common.RegisterContainers;
using IdentityServer4.AccessTokenValidation;
using MassTransit;
using MassTransit.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace Appointment.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRabbitMqMessageBus(Configuration);
            services.AddMongoDB(Configuration);
            //services.AddRabbitMq(Configuration);

            Appointment.Infrastructure.Configure.ConfigureServices(services);
            services.AddMvc(option => option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            var serviceHost = Configuration.GetSection(nameof(ServiceHost));
            services.Configure<ServiceHost>(serviceHost);
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.JwtBearerEvents = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.Response.StatusCode = 401;
                            return Task.CompletedTask;
                        }
                    };
                    options.Authority = serviceHost[nameof(ServiceHost.SecurityAPI)];
                    options.RequireHttpsMetadata = false;
                });

         

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            loggerFactory.AddProvider(new MicroservicesLoggerProvider(serviceProvider.GetService<IMessageBus>(), Configuration));
            app.UseAuthentication();
            app.UseHttpsRedirection();
            var logger = serviceProvider.GetService<ILogger<Startup>>();
            app.UseErrorLogging(logger);
            app.UseMvc();
        }
    }
}
