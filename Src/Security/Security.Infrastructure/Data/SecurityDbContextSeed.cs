using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Security.Core.Entities;

namespace Security.Infrastructure.Data
{
    public class SecurityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<User> userManager)
        {
            var defaultUser = new User(
                "demoFirst",
                "DemoLast",
                "demouser@microsoft.com",
                "demouser@microsoft.com");
            await userManager.CreateAsync(defaultUser, "Pass@word1");
        }
    }
}
