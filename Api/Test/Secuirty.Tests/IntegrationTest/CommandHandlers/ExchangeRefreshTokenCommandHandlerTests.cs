using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.General.UnitOfWork;
using IntegrationTest.Builders;
using Microsoft.EntityFrameworkCore;
using Security.API.Commands.ExchangeRefreshToken;
using Security.Infrastructure.Data;
using Security.Infrastructure.Data.Repositories;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.CommandHandlers
{
    public class ExchangeRefreshTokenCommandHandlerTests
    {
        private readonly UserRepository _userRepository;
        private readonly SecurityDbContext _dbContext;
        private UserBuilder UserBuilder { get; } = new UserBuilder();
        private readonly ITestOutputHelper _output;
        private readonly ExchangeRefreshTokenCommandHandler _commandHandler;
        public ExchangeRefreshTokenCommandHandlerTests(ITestOutputHelper output)
        {
            _output = output;
            var dbOptions = new DbContextOptionsBuilder<SecurityDbContext>()
                .UseInMemoryDatabase(databaseName: "TestSecurity")
                .Options;
            _dbContext = new SecurityDbContext(dbOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(_dbContext);
            _userRepository = new UserRepository(unitOfWork);
            _commandHandler = new ExchangeRefreshTokenCommandHandler(_userRepository, unitOfWork);
        }
        [Fact]
        public async Task ExchangeRefreshTokenCommandHandler_GivenValidCommand_ShouldInsertedInDb()
        {
            var existingUser = UserBuilder.WithOneRefreshToken();
            _dbContext.Users.Add(existingUser);
            _dbContext.SaveChanges();
            var userId = existingUser.Id;
            _output.WriteLine($"userId: {userId}");

            var userFromRepository = await _userRepository.GetByIdAsync(userId);
            UserBuilder.UserId.ShouldBe(userFromRepository.Email);


            var firstItem = userFromRepository.RefreshTokens.FirstOrDefault();
            firstItem.ShouldNotBeNull();
            UserBuilder.refreshToken.ShouldBe(firstItem.Token);

            var newRefreshToken = "newRefreshToken";
            await _commandHandler.Handle(new ExchangeRefreshTokenCommand(
                firstItem.UserId, newRefreshToken, firstItem.Token),
                CancellationToken.None);
            var userAfterUpdated = await _userRepository.GetByIdAsync(userId);
            var tokenAfterUpdated = userAfterUpdated.RefreshTokens.FirstOrDefault();
            tokenAfterUpdated.ShouldNotBeNull();
            tokenAfterUpdated.Token.ShouldBe(newRefreshToken);
        }
    }
}
