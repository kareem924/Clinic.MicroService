using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Security.API.Models;
using Security.Core.Dto;
using Security.Core.UseCases;

namespace Security.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginUseCase _loginUseCase;
        private readonly IExchangeRefreshTokenUseCase _exchangeRefreshTokenUseCase;
        private readonly AuthSettings _authSettings;

        public AuthController(
            ILoginUseCase loginUseCase,
            IExchangeRefreshTokenUseCase exchangeRefreshTokenUseCase,
            IOptions<AuthSettings> authSettings)
        {
            _loginUseCase = loginUseCase;
            _exchangeRefreshTokenUseCase = exchangeRefreshTokenUseCase;
            _authSettings = authSettings.Value;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto request)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var token = await _loginUseCase.Handle(
                new LoginDto(
                    request.UserName,
                    request.Password,
                    Request.HttpContext.Connection.RemoteIpAddress?.ToString()));
            return Ok(token);
        }

        [HttpPost("refreshToken")]
        public async Task<ActionResult> RefreshToken([FromBody] ExchangeRefreshTokenDto request)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var refreshToken = await _exchangeRefreshTokenUseCase.Handle(
                new ExchangeRefreshTokenDto(request.AccessToken, request.RefreshToken, _authSettings.SecretKey));
            return Ok(refreshToken);
        }
    }
}