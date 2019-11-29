using System;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Security.API.Dto;
using System.Threading.Tasks;
using Common.RegisterContainers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Security.API.Application.Commands.ExchangeRefreshToken;
using Security.API.Application.Commands.UpdateUserRefreshToken;
using Security.API.Application.Queries.GetUserById;
using Security.API.Application.Queries.GetUserByUserName;
using Security.Infrastructure.Interfaces;
using Security.API.Application.Commands.RegisterUser;

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
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IMediator mediator,
            IJwtFactory jwtFactory,
            ITokenFactory tokenFactory,
            IJwtTokenValidator jwtTokenValidator,
            IOptions<AuthSettings> authSettings,
            ILogger<AccountController> logger)
        {
            _mediator = mediator;
            _jwtFactory = jwtFactory;
            _tokenFactory = tokenFactory;
            _jwtTokenValidator = jwtTokenValidator;
            _logger = logger;
            _authSettings = authSettings.Value;
        }


        [HttpGet("token")]
        public async Task<ActionResult<TokenResponseDto>> GenerateToken([FromQuery]TokenRequestDto request)
        {
            var user = await _mediator.Send(new GetLoginUserQuery(request.UserName, request.Password));
            if (user == null)
            {
                _logger.LogWarning("User is null");
                return Unauthorized(new TokenResponseDto(null, ""));
            }
            var refreshToken = _tokenFactory.GenerateToken();
            await _mediator.Publish(new UpdateUserRefreshTokenCommand(
                user.Id,
                refreshToken,
                Request.HttpContext.Connection.RemoteIpAddress?.ToString()));
            return Ok(new TokenResponseDto(
                await _jwtFactory.GenerateEncodedToken(user),
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
            var jwtToken = await _jwtFactory.GenerateEncodedToken(user);
            var refreshToken = _tokenFactory.GenerateToken();
            await _mediator.Publish(new ExchangeRefreshTokenCommand(
                user.Id,
                refreshToken,
                request.RefreshToken));
            return Ok(new ExchangeRefreshTokenResponseDto(jwtToken, refreshToken, true));
        }

        [HttpPost]
        public async Task<ActionResult<ExchangeRefreshTokenResponseDto>> Register([FromBody]SignUpRequestDto request)
        {
            await _mediator.Publish(new RegisterUserCommand(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Street,
                request.State,
                request.City,
                request.Country,
                request.BirthDate,
                request.PhoneNumber,
                request.Password));
            return Ok();
        }
    }
}