using System;
using Security.Core.Entities;

namespace IntegrationTest.Builders
{
   public class UserBuilder
   {
       private User _user;
       private readonly  Guid _userId = Guid.NewGuid();

       public UserBuilder()
       {
           
       }

       public User WithNoItems()
       {
           _user = new User {  Id = _userId };
           return _user;
       }

       public User WithOneRefreshToken()
       {
           const string refreshToken = "1234";
            _user = new User("firstName", "", "", "email", true, null, DateTime.MaxValue);
           _user.AddRefreshToken(refreshToken, Guid.NewGuid(), "127.0.0.1");
            return _user;
       }
    }
}
