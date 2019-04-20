using System;
namespace Abstract.Security.Entities
{
    public partial class UserTokens
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string LoginProvider { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }

    }
}
