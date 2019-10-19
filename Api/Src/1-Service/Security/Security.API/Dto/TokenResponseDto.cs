using Common.General.Dto.Result;
using Security.Core.Dto;
using System.Collections.Generic;

namespace Security.API.Dto
{
    public class TokenResponseDto : Result
    {
        public TokenResponseDto(IEnumerable<Error> errors, bool success = false, string message = null) :
            base(success, new[] { message }, errors)
        {
            Errors = errors;
        }
        public TokenResponseDto()
        {

        }
        public TokenResponseDto(AccessTokenDto accessToken, string refreshToken, bool success = false, string message = null)
            : base(success, new[] { message })
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public AccessTokenDto AccessToken { get; set; }

        public string RefreshToken { get; set; }
        public bool Success { get; set; }

        public IEnumerable<Error> Errors { get; set; }

    }
}
