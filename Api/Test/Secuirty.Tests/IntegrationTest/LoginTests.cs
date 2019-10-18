﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Security.Core.Entities;
using Security.Infrastructure.Data;
using Xunit;

namespace IntegrationTest
{
    public class LoginTests
    {
        [Fact]
        public async Task LogsInSampleUser()
        {
            var services = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase();
            services.AddDbContext<SecurityDbContext>(options =>
            {
                options.UseInMemoryDatabase("Identity");
            });
            services.AddIdentity<User, Role>(options =>
                {
                    options.User.RequireUniqueEmail = false;
                })
                .AddEntityFrameworkStores<SecurityDbContext>()
                .AddDefaultTokenProviders();
            services.AddLogging();
            var serviceProvider = services
                .BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;

                try
                {
                    // seed sample user data
                    var userManager = scopedServices.GetRequiredService<UserManager<User>>();

                    var defaultUser = new User(
                        "demoFirst",
                        "DemoLast",
                        "demouser@microsoft.com",
                        "demouser@microsoft.com",
                        true, null, DateTime.MaxValue);
                    await userManager.CreateAsync(defaultUser, "Pass@word1");
                    var result = await userManager.CheckPasswordAsync(defaultUser, "Pass@word1");
                    Assert.True(result);

                }
                catch (Exception ex)
                {
                    Assert.True(string.IsNullOrEmpty(ex.Message));
                }
            }

        }
    }
}
