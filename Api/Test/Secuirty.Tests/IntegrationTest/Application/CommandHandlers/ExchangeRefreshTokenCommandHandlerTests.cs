using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.General.UnitOfWork;
using IntegrationTest.Builders;
using Microsoft.EntityFrameworkCore;
using Security.Infrastructure.Application.Commands.ExchangeRefreshToken;
using Security.Infrastructure.Data;
using Security.Infrastructure.Data.Repositories;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.Application.CommandHandlers
{
    public class ExchangeRefreshTokenCommandHandlerTests
    {

        private UserBuilder UserBuilder { get; } = new UserBuilder();
        private readonly ITestOutputHelper _output;
        private readonly StartUp _startUp;
        public ExchangeRefreshTokenCommandHandlerTests(ITestOutputHelper output)
        {
            _output = output;
            _startUp = new StartUp();
        }
        [Fact]
        public async Task ExchangeRefreshTokenCommandHandler_GivenValidCommand_ShouldInsertedInDb()
        {
            var newRefreshToken = "newRefreshToken";
            var existingUser = UserBuilder.WithOneRefreshToken();
            _startUp.DbContext.Users.Add(existingUser);
            _startUp.DbContext.SaveChanges();
            var userId = existingUser.Id;
            _output.WriteLine($"userId: {userId}");
            var userRepository = new UserRepository(_startUp.UnitOfWork);
            var userFromRepository = await userRepository.GetByIdAsync(userId);
            var firstItem = userFromRepository.RefreshTokens.FirstOrDefault();
            var commandHandler = new ExchangeRefreshTokenCommandHandler(userRepository, _startUp.UnitOfWork);
            await commandHandler.Handle(new ExchangeRefreshTokenCommand(
                firstItem.UserId, newRefreshToken, firstItem.Token),
                CancellationToken.None);

            var userAfterUpdated = await userRepository.GetByIdAsync(userId);
            var tokenAfterUpdated = userAfterUpdated.RefreshTokens.FirstOrDefault();

            firstItem.ShouldNotBeNull();
            UserBuilder.RefreshToken.ShouldBe(firstItem.Token);
            UserBuilder.UserId.ShouldBe(userFromRepository.Email);
            tokenAfterUpdated.ShouldNotBeNull();
            tokenAfterUpdated.Token.ShouldBe(newRefreshToken);
        }
    }
}
