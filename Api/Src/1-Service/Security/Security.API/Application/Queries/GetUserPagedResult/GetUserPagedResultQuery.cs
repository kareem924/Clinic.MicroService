using Common.CQRS;
using Common.General.Entity;
using Security.Core.Entities;

namespace Security.API.Application.Queries.GetUserPagedResult
{
    public class GetUserPagedResultQuery : PagedQueryBase,IQuery<PagedResult<UserDto>>
    {
        public string Name { get;  set; }

        public string Email { get;  set; }

        public GetUserPagedResultQuery()
        {
            
        }
       
    }
}
