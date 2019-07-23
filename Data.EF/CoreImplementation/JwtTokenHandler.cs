using System.IdentityModel.Tokens.Jwt;
using Auth.Infrastructure.CoreImplementation.Interfaces;
using Microsoft.Extensions.Logging;

namespace Auth.Infrastructure.CoreImplementation
{
    internal sealed class JwtTokenHandler : IJwtTokenHandler
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly ILogger _logger;

        internal JwtTokenHandler(ILogger logger)
        {
            if (_jwtSecurityTokenHandler == null)
                _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            _logger = loger;
        }

        public string WriteToken(JwtSecurityToken jwt)
        {
            return _jwtSecurityTokenHandler.WriteToken(jwt);
        }


    }
}
