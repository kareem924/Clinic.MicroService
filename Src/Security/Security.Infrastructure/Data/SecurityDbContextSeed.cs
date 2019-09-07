using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Security.Core.Entities;
using Security.Infrastructure.Helper;

namespace Security.Infrastructure.Data
{
    public static class SecurityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<User> userManager)
        {
            var defaultUser = new User(
                "demoFirst",
                "DemoLast",
                "demouser@microsoft.com",
                "demouser@microsoft.com",
                true);
            await userManager.CreateAsync(defaultUser, "Pass@word1");
        }
        public static async Task SeedRolesAsync(RoleManager<Role> roleManager)
        {
            var values = typeof(Roles).GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(x => x.IsLiteral && !x.IsInitOnly)
                .Select(x => x.GetValue(null)).Cast<string>();
            Type type = typeof(Roles); // MyClass is static class with static properties
            foreach (var value in values)
            {
                var defaultRole = new Role(value.ToString());
                await roleManager.CreateAsync(defaultRole);
            }

        }
    }
}
