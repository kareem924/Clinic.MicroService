using System;
using System.Collections.Generic;
using System.Text;
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
using Security.Infrastructure.Application.Commands.UpdateUserCommand;
using Security.Infrastructure.Application.Commands.UpdateUserRefreshToken;
using Security.Infrastructure.Application.Dto;
using Security.Infrastructure.Data;
using Security.Infrastructure.Data.Repositories;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Application.CommandHandlers
{
   public class UpdateUserCommandHandlerTests
    {
        private  UserRepository _userRepository;
        private  SecurityDbContext _dbContext;
        private UserBuilder UserBuilder { get; } = new UserBuilder();
        private readonly ITestOutputHelper _output;
        private UpdateUserCommandHandler _commandHandler;
        private ILoggerFactory _logging;
        private  IServiceCollection _services;

        public UpdateUserCommandHandlerTests(ITestOutputHelper output)
        {
            _output = output;
            _services = new ServiceCollection();
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
            var dbOptions = new DbContextOptionsBuilder<SecurityDbContext>()
                .UseInMemoryDatabase(databaseName: "TestSecurity")
                .Options;
            _services.AddLogging();
          
        }

        [Fact]
        public async Task UpdateUserCommandHandler_GivenValidCommand_ShouldInsertedInDb()
        {
            var serviceProvider = _services
                .BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;

                try
                {

                    var userManager = scopedServices.GetRequiredService<UserManager<User>>();
                    _logging = scopedServices.GetRequiredService<ILoggerFactory>();
                    var roleManager = scopedServices.GetRequiredService<RoleManager<Role>>();
                    await SecurityDbContextSeed.SeedRolesAsync(roleManager);
                    await SecurityDbContextSeed.SeedUserAsync(userManager);
                    _logging = scopedServices.GetRequiredService<ILoggerFactory>();
                    _dbContext = scopedServices.GetRequiredService<SecurityDbContext>();
                    IUnitOfWork unitOfWork = new UnitOfWork(_dbContext);
                    _userRepository = new UserRepository(unitOfWork);

                    var userToUpdate = await userManager.FindByEmailAsync("demouser@microsoft.com");
                    _commandHandler = new UpdateUserCommandHandler(_userRepository, _logging.CreateLogger<UpdateUserCommandHandler>(), unitOfWork);
                    await _commandHandler.Handle(
                        new UpdateUserCommand()
                        {
                            Id = userToUpdate .Id,
                            BirthDate = DateTime.Now.AddDays(-1000),
                            Email = "demouser@microsoft.eg",
                            EmailConfirmed = true,
                            FirstName = "Amina",
                            IsActive = true,
                            LastName = "Kamel",
                            PhoneNumber = "0233808243",
                            Roles = new List<RoleDto>() {new RoleDto() {Name = "doctor" }, new RoleDto() { Name = "admin" } }
                        },
                        CancellationToken.None);
                    var updatedUser = await _userRepository.FindAsync(new UserSpecification("demouser@microsoft.eg"));
                    updatedUser.ShouldNotBeNull();
                }
                catch (Exception ex)
                {
                    string.IsNullOrEmpty(ex.Message).ShouldBeTrue();
                }
            }
        }
    }
}
