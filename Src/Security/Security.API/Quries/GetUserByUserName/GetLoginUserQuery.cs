using Common.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Security.API.Quries.GetUserByUserName
{
    public class GetLoginUserQuery: IQuery<LoginUserDto>
    {
        public string UserName { get; private set; }

        public string Password { get; private set; }

        public GetLoginUserQuery(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
