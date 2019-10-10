using MediatR;
using Microsoft.AspNetCore.Mvc;
using Security.API.Dto;
using Security.API.Quries.GetUserByUserName;
using System.Threading.Tasks;

namespace Security.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            this._mediator = mediator;
        }


        [HttpGet("token")]
        public async Task<ActionResult<TokenResponseDto>> GenerateToken([FromQuery]TokenRequestDto request)
        {
            var user = await _mediator.Send(new GetLoginUserQuery(request.UserName, request.Password));
            return Ok(user);
        }
    }
}