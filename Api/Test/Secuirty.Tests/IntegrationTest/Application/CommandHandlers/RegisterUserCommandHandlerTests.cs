using System;
using System.Threading;
using System.Threading.Tasks;
using Common.General.UnitOfWork;
using IntegrationTest.Builders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Security.Core.Entities;
using Security.Core.Specification;
using Security.Infrastructure.Application.Commands.RegisterUser;
using Security.Infrastructure.Data;
using Security.Infrastructure.Data.Repositories;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Application.CommandHandlers
{
    public class RegisterUserCommandHandlerTests
    {
        private UserRepository _userRepository;
        private SecurityDbContext _dbContext;
        private UserBuilder UserBuilder { get; } = new UserBuilder();
        private readonly ITestOutputHelper _output;
        private RegisterUserCommandHandler _commandHandler;
        private ILoggerFactory _logging;
        private readonly IServiceCollection _services;
        public RegisterUserCommandHandlerTests(
            ITestOutputHelper output)
        {
            _output = output;
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
                    var roleManager = scopedServices.GetRequiredService<RoleManager<Role>>();
                    await SecurityDbContextSeed.SeedRolesAsync(roleManager);

                    _commandHandler = new RegisterUserCommandHandler(userManager, _logging.CreateLogger<RegisterUserCommandHandler>());

                    _dbContext = scopedServices.GetRequiredService<SecurityDbContext>();
                    IUnitOfWork unitOfWork = new UnitOfWork(_dbContext);
                    _userRepository = new UserRepository(unitOfWork);
                    await _commandHandler.Handle(new RegisterUserCommand("test", "test2", "test@test.test", "15 sh", "dokki", "giza", "egypt", DateTime.Now, "", "P@ssw0rd"), CancellationToken.None);
                    var createdUser = await _userRepository.FindAsync(new UserSpecification("test@test.test"));
                    createdUser.ShouldNotBeNull();
                }
                catch (Exception ex)
                {
                    string.IsNullOrEmpty(ex.Message).ShouldBeTrue();
                }
            }
        }
    }
}
