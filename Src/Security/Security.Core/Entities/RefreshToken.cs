using System;
using System.Collections.Generic;
using System.Text;
using Common.General.Entity;

namespace Security.Core.Entities
{
    public class RefreshToken : FullTrackInfoEntity<Guid>
    {

        public string Token { get; private set; }

        public DateTime Expires { get; private set; }

        public Guid UserId { get; private set; }

        public bool Active => DateTime.UtcNow <= Expires;

        public string RemoteIpAddress { get; private set; }

        public bool IsDeleted { get; set; }

        public RefreshToken(string token, DateTime expires, Guid userId, string remoteIpAddress)
        {
            Token = token;
            Expires = expires;
            UserId = userId;
            RemoteIpAddress = remoteIpAddress;
        }


    }
}
