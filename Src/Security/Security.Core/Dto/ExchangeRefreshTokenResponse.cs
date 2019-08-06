using System;
using System.Collections.Generic;
using System.Text;
using Common.General.Dto.Result;

namespace Security.Core.Dto
{
    public class ExchangeRefreshTokenResponse : Result
    {
        public AccessToken AccessToken { get; }

        public string RefreshToken { get; }



        public ExchangeRefreshTokenResponse(IEnumerable<Error> errors, bool success = false, string message = null) :
            base(success, new[] { message }, errors)
        {
        }

        public ExchangeRefreshTokenResponse(
            AccessToken accessToken,
            string refreshToken,
            bool success = true,
            string message = null) : base(success: success, message: new[] { message })
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
