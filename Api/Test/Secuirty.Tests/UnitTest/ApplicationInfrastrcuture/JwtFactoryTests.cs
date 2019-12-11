using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Common.RegisterContainers;
using Common.Security;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Security.Core.Entities;
using Security.Infrastructure.Interfaces;
using Security.Infrastructure.Service;
using Shouldly;
using Xunit;

namespace UnitTest.ApplicationInfrastrcuture
{
    public class JwtFactoryUnitTests
    {
        [Fact]
        public async void GenerateEncodedToken_GivenValidInputs_ReturnsExpectedTokenData()
        {
            // arrange
            var token = Guid.NewGuid().ToString();
            var id = Guid.NewGuid().ToString();
            var jwtIssuerOptions = new JwtIssuerOptions
            {
                Issuer = "",
                Audience = "",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes("secret_key")),
                    SecurityAlgorithms.HmacSha256)
            };
            var mockJwtTokenHandler = new Mock<IJwtTokenHandler>();
            mockJwtTokenHandler.Setup(handler => handler
                    .WriteToken(It.IsAny<JwtSecurityToken>()))
                .Returns(token);
            var jwtFactory = new JwtFactory(mockJwtTokenHandler.Object, Options.Create(jwtIssuerOptions));

            // act
            var result = await jwtFactory.GenerateEncodedToken(new User());

            // assert
            result.Token.ShouldBe(token);
        }
    }
}
