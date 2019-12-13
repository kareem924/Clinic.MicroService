using System;
using Common.CQRS;
using Security.Infrastructure.Application.Dto;

namespace Security.Infrastructure.Application.Queries.GetUserDtoId
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
