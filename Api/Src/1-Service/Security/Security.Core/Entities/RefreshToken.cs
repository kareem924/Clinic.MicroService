using System;
using Common.General.Entity;

namespace Security.Core.Entities
{
    public class RefreshToken : GuidIdEntity
    {
        public string Token { get; private set; }

        public DateTime Expires { get; private set; }

        public Guid UserId { get; private set; }

        public bool Active => DateTime.UtcNow <= Expires;

        public string RemoteIpAddress { get; private set; }

        public User User { get; set; }


        public RefreshToken()
        {
        }
        public RefreshToken(string token, DateTime expires, Guid userId, string remoteIpAddress)
        {
            Token = token;
            Expires = expires;
            UserId = userId;
            RemoteIpAddress = remoteIpAddress;
        }
    }
}
