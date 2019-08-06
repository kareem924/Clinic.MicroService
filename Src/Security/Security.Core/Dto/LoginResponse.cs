using System;
using System.Collections.Generic;
using System.Text;
using Common.General.Dto.Result;

namespace Security.Core.Dto
{
    public class LoginResponse : Result
    {
        public AccessToken AccessToken { get; }

        public string RefreshToken { get; }



        public LoginResponse(IEnumerable<Error> errors, bool success = false, string message = null) :
            base(success, new[] { message }, errors)
        {
        }

        public LoginResponse(
            AccessToken accessToken,
            string refreshToken,
            bool success = true,
            string message = null)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
