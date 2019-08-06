using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Core.Dto
{
    public class ExchangeRefreshTokenDto
    {
        public string AccessToken { get; }
        public string RefreshToken { get; }
        public string SigningKey { get; }

        public ExchangeRefreshTokenDto(string accessToken, string refreshToken, string signingKey)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            SigningKey = signingKey;
        }
    }
}
