using System;
using System.Collections.Generic;
using System.Text;
using Common.General.Dto.Result;

namespace Security.Core.Dto
{
    public class LoginResponse
    {
        public AccessToken AccessToken { get; }

        public string RefreshToken { get; }

        public Result Errors { get; }

        public LoginResponse(Result errors, bool success = false, string message = null)
        {
            Errors = errors;
        }

        public LoginResponse(AccessToken accessToken, string refreshToken, bool success = false, string message = null)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
