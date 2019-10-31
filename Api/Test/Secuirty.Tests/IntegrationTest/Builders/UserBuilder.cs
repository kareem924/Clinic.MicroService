using System;
using Security.Core.Entities;

namespace IntegrationTest.Builders
{
   public class UserBuilder
   {
       private User _user;
       public  string UserId = "testEmail";
       public const string refreshToken = "1234";
        public UserBuilder()
       {
           
       }

       public User WithNoItems()
       {
           _user = new User {  Email = UserId };
           return _user;
       }

       public User WithOneRefreshToken()
       {
            _user = new User("firstName", "", "", UserId, true, null, DateTime.MaxValue,string.Empty);
           _user.AddRefreshToken(refreshToken, Guid.NewGuid(), "127.0.0.1");
            return _user;
       }
    }
}
