using System;
using System.Collections.Generic;
using System.Text;
using Common.General.Entity;

namespace Security.Core.Entities
{
    public class RefreshToken : IFullTrackInfoEntity<Guid>
    {
        public Guid Id { get; set; }

        public string Token { get; private set; }

        public DateTime Expires { get; private set; }

        public int UserId { get; private set; }

        public bool Active => DateTime.UtcNow <= Expires;

        public string RemoteIpAddress { get; private set; }

        public bool IsDeleted { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public Guid UpdateBy { get; set; }

        public DateTime UpdatingDate { get; set; }

        public RefreshToken(string token, DateTime expires, int userId, string remoteIpAddress)
        {
            Token = token;
            Expires = expires;
            UserId = userId;
            RemoteIpAddress = remoteIpAddress;
        }


    }
}
