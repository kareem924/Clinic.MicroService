using Common.CQRS;
using Security.Core.Entities;

namespace Security.Infrastructure.Application.Queries.GetUserByUserName
{
    public class GetLoginUserQuery: IQuery<User>
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
