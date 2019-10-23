using System;
using Common.CQRS;

namespace Security.API.Application.Commands.UpdateUserRefreshToken
{
    public class UpdateUserRefreshTokenCommand : ICommand
    {
        public string RefreshToken { get; private set; }

        public Guid UserId { get; private set; }

        public string RemoteIpAddress { get; private set; }

        public UpdateUserRefreshTokenCommand(Guid userId, string refreshToken, string remoteIpAddress)
        {
            RefreshToken = refreshToken;
            UserId = userId;
            RemoteIpAddress = remoteIpAddress;
        }
    }
}
