using System;


namespace Abstract.Security.Entities
{
    public class RefreshToken
    {

        public int Id { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }
    }
}
