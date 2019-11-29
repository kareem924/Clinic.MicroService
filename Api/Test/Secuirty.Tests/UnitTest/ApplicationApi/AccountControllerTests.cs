using System.Threading.Tasks;
using Common.RegisterContainers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Security.API.Controllers;
using Security.API.Dto;
using Security.Infrastructure.Interfaces;
using Xunit;

namespace UnitTest.ApplicationApi
{
  public  class AccountControllerTests
    {
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            var mediator = new Mock<IMediator>();
            var jwtFactory = new Mock<IJwtFactory>();
            var tokenFactory = new Mock<ITokenFactory>();
            var jwtTokenValidator = new Mock<IJwtTokenValidator>();
            var authSettings = new Mock<IOptions<AuthSettings>>();
            var logger = new Mock<ILogger<AccountController>>();
            _controller = new AccountController(
                mediator.Object,
                jwtFactory.Object,
                tokenFactory.Object,
                jwtTokenValidator.Object,
                authSettings.Object,
                logger.Object);
        }
        [Fact]
        public async Task Get_WhenCalled_ReturnsBadRequest()
        {
            // Act
            var badRequestResult = await _controller.GenerateToken(new TokenRequestDto());
            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult.Result);
        }
      
       
        }
}
