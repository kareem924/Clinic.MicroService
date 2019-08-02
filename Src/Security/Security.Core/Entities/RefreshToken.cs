using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Core.Entities
{
    public class RefreshToken
    {
        public string Token { get; private set; }
        public DateTime Expires { get; private set; }
        public int UserId { get; private set; }
        public bool Active => DateTime.UtcNow <= Expires;
        public string RemoteIpAddress { get; private set; }

        public RefreshToken(string token, DateTime expires, int userId, string remoteIpAddress)
        {
            Token = token;
            Expires = expires;
            UserId = userId;
            RemoteIpAddress = remoteIpAddress;
        }
    }
}
