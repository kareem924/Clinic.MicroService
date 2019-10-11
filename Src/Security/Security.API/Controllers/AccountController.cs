using MediatR;
using Microsoft.AspNetCore.Mvc;
using Security.API.Dto;
using Security.API.Queries.GetUserByUserName;
using System.Threading.Tasks;
using Security.API.Commands.UpdateUserRefreshToken;
using Security.Core.Dto;
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

        public AccountController(IMediator mediator, IJwtFactory jwtFactory, ITokenFactory tokenFactory)
        {
            _mediator = mediator;
            _jwtFactory = jwtFactory;
            _tokenFactory = tokenFactory;
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
    }
}