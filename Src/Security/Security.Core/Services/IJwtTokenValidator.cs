using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Security.Core.Services
{
    public interface IJwtTokenValidator
    {
        ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey);
    }
}
