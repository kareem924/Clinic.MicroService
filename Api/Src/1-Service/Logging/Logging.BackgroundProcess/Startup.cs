using System;
using System.Reflection;
using Common.MongoDb;
using Common.RabbitMq;
using Common.RegisterContainers;
using Logging.BackgroundProcess.Consumers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Logging.BackgroundProcess
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
            Infrastructure.Configure.ConfigureServices(services, Configuration);
            services.AddMongoDB(Configuration);
            //HandlerRegister.Register(Assembly.GetExecutingAssembly(), services, Configuration);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider)
        {
            app.ConfigureAppBuilderExt(loggerFactory, serviceProvider, Configuration, Assembly.GetExecutingAssembly());
            app.UseAuthentication();
            app.UseHttpsRedirection();        }
    }
}
