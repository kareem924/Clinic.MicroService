using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.General.UnitOfWork;
using IntegrationTest.Builders;
using Microsoft.EntityFrameworkCore;
using Security.API.Application.Commands.UpdateUserRefreshToken;
using Security.Infrastructure.Data;
using Security.Infrastructure.Data.Repositories;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.CommandHandlers
{
   public class UpdateUserRefreshTokenCommandHandlerTests
    {
        private readonly UserRepository _userRepository;
        private readonly SecurityDbContext _dbContext;
        private UserBuilder UserBuilder { get; } = new UserBuilder();
        private readonly ITestOutputHelper _output;
        private readonly UpdateUserRefreshTokenCommandHandler _commandHandler;
        public UpdateUserRefreshTokenCommandHandlerTests(ITestOutputHelper output)
        {
            _output = output;
            var dbOptions = new DbContextOptionsBuilder<SecurityDbContext>()
                .UseInMemoryDatabase(databaseName: "TestSecurity")
                .Options;
            _dbContext = new SecurityDbContext(dbOptions);
            IUnitOfWork unitOfWork = new UnitOfWork(_dbContext);
            _userRepository = new UserRepository(unitOfWork);
            _commandHandler = new UpdateUserRefreshTokenCommandHandler(_userRepository, unitOfWork);
        }
        [Fact]
        public async Task UpdateUserRefreshTokenCommandHandler_GivenValidCommand_ShouldInsertedInDb()
        {
            var existingUser = UserBuilder.WithNoItems();
            _dbContext.Users.Add(existingUser);
            _dbContext.SaveChanges();
            var userId = existingUser.Id;
            _output.WriteLine($"userId: {userId}");

            await _commandHandler.Handle(
                new UpdateUserRefreshTokenCommand(
                    existingUser.Id,
                    UserBuilder.refreshToken,
                    string.Empty),
                CancellationToken.None);
            var userAfterInserted = await _userRepository.GetByIdAsync(userId);
            var tokenAfterInserted = userAfterInserted.RefreshTokens.FirstOrDefault();

            tokenAfterInserted.ShouldNotBeNull();
            tokenAfterInserted.Token.ShouldBe(UserBuilder.refreshToken);
        }
    }
}
