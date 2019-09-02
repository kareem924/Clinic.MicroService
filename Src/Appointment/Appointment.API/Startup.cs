using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.General.Dto;
using Common.MongoDb;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddMongoDB(Configuration);

            var serviceHost = Configuration.GetSection(nameof(ServiceHost));
            services.Configure<ServiceHost>(serviceHost);

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                   .AddIdentityServerAuthentication(options =>
                   {
                       options.Authority = serviceHost[nameof(ServiceHost.SecurityAPI)];
                       options.RequireHttpsMetadata = false;
                       options.ApiName = nameof(ServiceHost.SecurityAPI);
                       options.ApiSecret = "secret";
                   });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
