using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Security.Core.Entities;
using Security.Infrastructure.Data;

namespace Security.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateWebHostBuilder(args)
                .Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                //var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    await SecurityDbContextSeed.SeedAsync(userManager);
                }
                catch (Exception ex)
                {
                    //var logger = loggerFactory.CreateLogger<Program>();
                    //logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }
              host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
