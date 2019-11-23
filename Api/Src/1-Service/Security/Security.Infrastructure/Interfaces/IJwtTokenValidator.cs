using System.Security.Claims;

namespace Security.Infrastructure.Interfaces
{
    public interface IJwtTokenValidator
    {
        ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey);
    }
}
