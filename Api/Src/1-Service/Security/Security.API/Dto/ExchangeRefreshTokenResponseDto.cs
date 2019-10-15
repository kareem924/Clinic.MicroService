using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.General.Dto.Result;
using Security.Core.Dto;

namespace Security.API.Dto
{
    public class ExchangeRefreshTokenResponseDto : Result
    {
        public ExchangeRefreshTokenResponseDto(IEnumerable<Error> errors, bool success = false, string message = null) :
            base(success, new[] { message }, errors)
        {
            Errors = errors;
        }

        public ExchangeRefreshTokenResponseDto(
            AccessTokenDto accessToken, 
            string refreshToken, 
            bool success = false, 
            string message = null)
            : base(success, new[] { message })
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public AccessTokenDto AccessToken { get; }

        public string RefreshToken { get; }

        public IEnumerable<Error> Errors { get; }

    }
}
