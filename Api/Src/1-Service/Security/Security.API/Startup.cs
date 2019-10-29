using System;
using System.Reflection;
using Common.Email;
using Common.General.Interfaces;
using Common.Loggings;
using Common.RabbitMq;
using Common.RegisterContainers;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Security.API.Application.Queries.GetUserByUserName;
using Security.Core.Entities;
using Security.Infrastructure.Data;
//using Swashbuckle.AspNetCore.Swagger;

namespace Security.API
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
            services.AddMvc(option => option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddFluentValidation(fv =>
                {
                    fv.ImplicitlyValidateChildProperties = false;
                });


            var sendGridKey = Configuration.GetSection("SendGrid");
            services.Configure<AuthMessageSenderOptions>(sendGridKey);



            services.AddTransient<IMapperService, MapperService>();
            Security.Infrastructure.Configure.ConfigureServices(
                services,
                Configuration.GetConnectionString("DefaultConnection"),
                Assembly.GetExecutingAssembly());

            //services.AddRabbitMq(Configuration);


            services.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = false;
            })
    .AddEntityFrameworkStores<SecurityDbContext>()
    .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                });

            services.AddRabbitMqMessageBus(Configuration, Assembly.GetExecutingAssembly());

            services.AddJwt(Configuration);
            //// Register the Swagger generator, defining 1 or more Swagger documents
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Info { Title = "AspNetCoreApiStarter", Version = "v1" });
            //    // Swagger 2.+ support
            //});


            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider)
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
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Security Clinic V1");
            //});

            //// Enable middleware to serve generated Swagger as a JSON endpoint.
            //app.UseSwagger();
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();
            //app.ConfigureAppBuilder(loggerFactory, serviceProvider, Configuration);
            loggerFactory.AddProvider(new MicroservicesLoggerProvider(serviceProvider.GetService<IMessageBus>(), Configuration));
            var logger = serviceProvider.GetService<ILogger>();
            app.UseErrorLogging(logger);

            app.UseMvc();
        }
    }
}
