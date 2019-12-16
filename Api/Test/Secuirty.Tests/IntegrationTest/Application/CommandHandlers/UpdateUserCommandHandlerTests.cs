using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IntegrationTest.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Security.Core.Specification;
using Security.Infrastructure.Application.Commands.UpdateUserCommand;
using Security.Infrastructure.Application.Dto;
using Security.Infrastructure.Data.Repositories;
using Security.Infrastructure.Helper;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Application.CommandHandlers
{
    public class UpdateUserCommandHandlerTests
    {
        private readonly StartUp _startUp;
        private readonly ITestOutputHelper _output;
        private UpdateUserCommandHandler _commandHandler;


        public UpdateUserCommandHandlerTests(ITestOutputHelper output)
        {
            _output = output;
            _startUp = new StartUp();
        }

        [Fact]
        public async Task UpdateUserCommandHandler_GivenValidCommand_ShouldInsertedInDb()
        {
            _startUp.InitializeDefaultUserInDb();
            var userRepository = new UserRepository(_startUp.UnitOfWork);
            var userToUpdate = await userRepository.FindAsync(new UserSpecification(UserBuilder.UserId));
            var userData = new UserBuilder().UserToUpdate();
            var factory = _startUp.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var logger = factory.CreateLogger<UpdateUserCommandHandler>();
            _commandHandler = new UpdateUserCommandHandler(userRepository, logger, _startUp.UnitOfWork);
            await _commandHandler.Handle(
                new UpdateUserCommand()
                {
                    Id = userToUpdate.Id,
                    BirthDate = userData.BirthDate,
                    Email = userData.Email,
                    EmailConfirmed = userData.EmailConfirmed,
                    FirstName = userData.FirstName,
                    IsActive = userData.IsActive,
                    LastName = userData.LastName,
                    PhoneNumber = userData.PhoneNumber,
                    Roles = userData.Roles.Select(role => new RoleDto() { Name = role.Role.Name })
                },
                CancellationToken.None);

            var updatedUser = await userRepository.FindAsync(new UserSpecification(userData.Email));

            updatedUser.ShouldNotBeNull();
            updatedUser.Email.ShouldBe(userData.Email);
            updatedUser.Roles.ShouldNotBeNull();
            updatedUser.Roles.First(userRole => userRole.Role.Name == Roles.Patient).ShouldNotBeNull();
            updatedUser.Roles.First(userRole => userRole.Role.Name == Roles.Admin).ShouldNotBeNull();
            Should.Throw<InvalidOperationException>(() =>
            {
                var notHasDoctorRole = updatedUser.Roles.First(userRole => userRole.Role.Name == Roles.Doctor);
            });
        }

    }
}

