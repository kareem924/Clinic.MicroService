using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Core.Dto
{
    public class LoginDto
    {
        public string UserName { get; }

        public string Password { get; }

        public string RemoteIpAddress { get; }

        public LoginDto(string userName, string password, string remoteIpAddress)
        {
            UserName = userName;
            Password = password;
            RemoteIpAddress = remoteIpAddress;
        }
    }
}
