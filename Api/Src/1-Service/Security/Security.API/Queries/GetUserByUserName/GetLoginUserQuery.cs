using Common.CQRS;

namespace Security.API.Queries.GetUserByUserName
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
