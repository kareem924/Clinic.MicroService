using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.General.UnitOfWork;
using IntegrationTest.Builders;
using Microsoft.EntityFrameworkCore;
using Security.Infrastructure.Application.Commands.UpdateUserRefreshToken;
using Security.Infrastructure.Data;
using Security.Infrastructure.Data.Repositories;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Application.CommandHandlers
{
    public class UpdateUserRefreshTokenCommandHandlerTests
    {
        private readonly StartUp _startUp;
        private UserBuilder UserBuilder { get; } = new UserBuilder();
        private readonly ITestOutputHelper _output;
        public UpdateUserRefreshTokenCommandHandlerTests(ITestOutputHelper output)
        {
            _startUp = new StartUp();
            _output = output;
        }
        [Fact]
        public async Task UpdateUserRefreshTokenCommandHandler_GivenValidCommand_ShouldInsertedInDb()
        {
            var existingUser = UserBuilder.WithNoItems();
            _startUp.DbContext.Users.Add(existingUser);
            _startUp.DbContext.SaveChanges();
            var userId = existingUser.Id;
            _output.WriteLine($"userId: {userId}");
            var userRepository = new UserRepository(_startUp.UnitOfWork);
            var commandHandler = new UpdateUserRefreshTokenCommandHandler(userRepository, _startUp.UnitOfWork);
            await commandHandler.Handle(
                new UpdateUserRefreshTokenCommand(
                    existingUser.Id,
                    UserBuilder.RefreshToken,
                    string.Empty),
                CancellationToken.None);

            var userAfterInserted = await userRepository.GetByIdAsync(userId);
            var tokenAfterInserted = userAfterInserted.RefreshTokens.FirstOrDefault();

            tokenAfterInserted.ShouldNotBeNull();
            tokenAfterInserted.Token.ShouldBe(UserBuilder.RefreshToken);
        }
    }
}
