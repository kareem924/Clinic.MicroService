using System;

namespace Abstract.Security.Entities
{
    public partial class UserClaims
    {
        public int Id { get; set; }
        public string ClaimName { get; set; }
        public string ClaimValue { get; set; }
        public int UserId { get; set; }

        public Users User { get; set; }
    }
}
