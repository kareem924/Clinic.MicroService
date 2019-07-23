using System;

namespace Auth.Core.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }

        public string Token { get; set; }

        public DateTime ExpireAt { get; set; }

        public Guid UserId { get; set; }

        public virtual User User { get; set; }

        public RefreshToken(string token, DateTime expireAt, Guid userId)
        {
            Token = token;
            ExpireAt = expireAt;
            UserId = userId;
        }
       
    }
}
