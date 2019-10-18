using System;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Security.API.Dto;
using Security.API.Queries.GetUserByUserName;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Security.API.Commands.ExchangeRefreshToken;
using Security.API.Commands.UpdateUserRefreshToken;
using Security.API.Models;
using Security.API.Queries.GetUserById;
using Security.Infrastructure.Interfaces;

namespace Security.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IJwtFactory _jwtFactory;
        private readonly ITokenFactory _tokenFactory;
        private readonly IJwtTokenValidator _jwtTokenValidator;
        private readonly AuthSettings _authSettings;

        public AccountController(
            IMediator mediator,
            IJwtFactory jwtFactory,
            ITokenFactory tokenFactory,
            IJwtTokenValidator jwtTokenValidator,
            IOptions<AuthSettings> authSettings)
        {
            _mediator = mediator;
            _jwtFactory = jwtFactory;
            _tokenFactory = tokenFactory;
            _jwtTokenValidator = jwtTokenValidator;
            _authSettings = authSettings.Value;
        }


        [HttpGet("token")]
        public async Task<ActionResult<TokenResponseDto>> GenerateToken([FromQuery]TokenRequestDto request)
        {
            var user = await _mediator.Send(new GetLoginUserQuery(request.UserName, request.Password));
            if (user == null)
            {
                return Ok(new TokenResponseDto(null, ""));
            }
            var refreshToken = _tokenFactory.GenerateToken();
            await _mediator.Publish(new UpdateUserRefreshTokenCommand(
                user.Id,
                refreshToken,
                Request.HttpContext.Connection.RemoteIpAddress?.ToString()));
            return Ok(new TokenResponseDto(
                await _jwtFactory.GenerateEncodedToken(user.Id.ToString(), user.FirstName),
                refreshToken,
                true));
        }

        [HttpGet("refresh-token")]
        public async Task<ActionResult<ExchangeRefreshTokenResponseDto>> GenerateRefreshToken(
            [FromQuery]ExchangeRefreshTokenRequestDto request)
        {
            var claimsPrincipal = _jwtTokenValidator.GetPrincipalFromToken(request.AccessToken, _authSettings.SecretKey);
            if (claimsPrincipal == null)
            {
                return BadRequest();
            }
            var userId = claimsPrincipal.Claims.First(c => c.Type == "id");
            var user = await _mediator.Send(new GetUserByIdQuery(Guid.Parse(userId.Value)));
            if (!user.HasValidRefreshToken(request.RefreshToken))
            {
                return BadRequest();
            }
            var jwtToken = await _jwtFactory.GenerateEncodedToken(user.Id.ToString(), user.UserName);
            var refreshToken = _tokenFactory.GenerateToken();
            await _mediator.Publish(new ExchangeRefreshTokenCommand(
                user.Id,
                refreshToken,
                request.RefreshToken));
            return Ok(new ExchangeRefreshTokenResponseDto(jwtToken, refreshToken, true));
        }
    }
}