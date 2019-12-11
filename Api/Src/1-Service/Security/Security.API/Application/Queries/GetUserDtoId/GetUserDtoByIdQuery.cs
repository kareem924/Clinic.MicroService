using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.CQRS;
using Security.API.Application.Queries.GetUserPagedResult;

namespace Security.API.Application.Queries.GetUserDtoId
{
    public class GetUserDtoByIdQuery : IQuery<UserDto>
    {
        public Guid UserId { get; private set; }

        public GetUserDtoByIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
