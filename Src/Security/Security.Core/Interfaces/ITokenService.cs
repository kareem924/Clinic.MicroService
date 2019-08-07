using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Security.Core.Dto;

namespace Security.Core.Interfaces
{
    public interface ITokenService
    {
        Task<ExchangeRefreshTokenResponse> GenerateRefreshToken(ExchangeRefreshTokenDto message);

        Task<LoginResponse> GetToken(LoginDto loginRequest);
    }
}
