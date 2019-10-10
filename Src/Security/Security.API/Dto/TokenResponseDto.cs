using Common.General.Dto.Result;
using Security.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Security.API.Dto
{
    public class TokenResponseDto : Result
    {
        public TokenResponseDto(IEnumerable<Error> errors, bool success = false, string message = null) :
            base(success, new[] { message }, errors)
        {
            Errors = errors;
        }

        public TokenResponseDto(AccessTokenDto accessToken, string refreshToken, bool success = false, string message = null)
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
