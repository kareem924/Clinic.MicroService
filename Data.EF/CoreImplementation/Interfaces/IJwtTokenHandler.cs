using System.IdentityModel.Tokens.Jwt;

namespace Auth.Infrastructure.CoreImplementation.Interfaces
{
    public interface IJwtTokenHandler
    {
        string WriteToken(JwtSecurityToken jwt);
       
    }
}
