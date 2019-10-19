using System.Collections.Generic;
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

        public ExchangeRefreshTokenResponseDto()
        {
            
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

        public AccessTokenDto AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public IEnumerable<Error> Errors { get; set; }

    }
}
