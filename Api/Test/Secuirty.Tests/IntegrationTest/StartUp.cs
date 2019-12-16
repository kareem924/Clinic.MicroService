using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common.General.UnitOfWork;
using IntegrationTest.Builders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Security.Core.Entities;
using Security.Infrastructure.Data;
using Security.Infrastructure.Data.Repositories;
using Security.Infrastructure.Helper;

namespace IntegrationTest
{
    public class StartUp
    {
        public ServiceCollection Services { get; private set; }
        public SecurityDbContext DbContext { get; private set; }
        public UnitOfWork UnitOfWork { get; private set; }
        public ServiceProvider ServiceProvider { get; private set; }

        public StartUp()
        {
            Services = new ServiceCollection();
            var dbOptions = new DbContextOptionsBuilder<SecurityDbContext>()
                .UseInMemoryDatabase(databaseName: "TestSecurity")
                .Options;
            DbContext = new SecurityDbContext(dbOptions);
            UnitOfWork = new UnitOfWork(DbContext);
             ServiceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();
        }

        public void InitializeDefaultUserInDb()
        {
            SeesRoles();
            var userRepository = new UserRepository(UnitOfWork);
            userRepository.Add(new UserBuilder().WithRoles());
            UnitOfWork.Commit();
        }


        private void SeesRoles()
        {
            var roleRepository = new RoleRepository(UnitOfWork);
            var values = typeof(Roles).GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(x => x.IsLiteral && !x.IsInitOnly)
                .Select(x => x.GetValue(null)).Cast<string>();
            Type type = typeof(Roles); // MyClass is static class with static properties
            foreach (var value in values)
            {
                var defaultRole = new Role(value.ToString());
                 roleRepository.Add(defaultRole);
            }
            UnitOfWork.Commit();
        }
    }
}
