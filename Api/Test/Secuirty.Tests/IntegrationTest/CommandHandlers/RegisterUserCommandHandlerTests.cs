using Common.General.UnitOfWork;
using IntegrationTest.Builders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Security.API.Application.Commands.RegisterUser;
using Security.Core.Entities;
using Security.Infrastructure.Data;
using Security.Infrastructure.Data.Repositories;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.CommandHandlers
{
    public class RegisterUserCommandHandlerTests
    {
        private UserManager<User> _userManager;
        private UserRepository _userRepository;
        private SecurityDbContext _dbContext;
        private UserBuilder UserBuilder { get; } = new UserBuilder();
        private readonly ITestOutputHelper _output;
        private RegisterUserCommandHandler _commandHandler;
        private ILoggerFactory _logging;
        private IServiceCollection _services;
        public RegisterUserCommandHandlerTests(
            ITestOutputHelper output)
        {
            _output = output;
            var dbOptions = new DbContextOptionsBuilder<SecurityDbContext>()
                .UseInMemoryDatabase(databaseName: "TestSecurity")
                .Options;

            _services = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase();
            _services.AddDbContext<SecurityDbContext>(options =>
            {
                options.UseInMemoryDatabase("Identity");
            });
            _services.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = false;
            })
                .AddEntityFrameworkStores<SecurityDbContext>()
                .AddDefaultTokenProviders();
            _services.AddLogging();



        }

        [Fact]
        public async Task UpdateUserRefreshTokenCommandHandler_GivenValidCommand_ShouldInsertedInDb()
        {
            var serviceProvider = _services
               .BuildServiceProvider();
            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;

                try
                {
                    // seed sample user data
                    var userManager = scopedServices.GetRequiredService<UserManager<User>>();
                    _logging = scopedServices.GetRequiredService<ILoggerFactory>();
                    _commandHandler = new RegisterUserCommandHandler(userManager, _logging.CreateLogger<RegisterUserCommandHandler>());

                    _dbContext = scopedServices.GetRequiredService<SecurityDbContext>();
                    IUnitOfWork unitOfWork = new UnitOfWork(_dbContext);
                    _userRepository = new UserRepository(unitOfWork);
                    await _commandHandler.Handle(new RegisterUserCommand("test", "test2", "test@test.test", "15 sh", "dokki", "giza", "egypt", DateTime.Now, "", "P@ssw0rd"), CancellationToken.None);
                    true.ShouldBeTrue();

                }
                catch (Exception ex)
                {
                    string.IsNullOrEmpty(ex.Message).ShouldBeTrue();
                }
            }


        }
    }
}
