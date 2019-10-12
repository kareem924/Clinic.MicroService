using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.CQRS;
using Security.Core.Entities;

namespace Security.API.Queries.GetUserById
{
    public class GetUserByIdQuery : IQuery<User>
    {
        public Guid UserId { get; private set; }

        public GetUserByIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
