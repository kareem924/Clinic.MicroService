using System;
using Common.CQRS;

namespace Security.API.Application.Commands.ExchangeRefreshToken
{
    public class ExchangeRefreshTokenCommand : ICommand
    {
        public Guid UserId { get; private set; }

        public string NewRefreshToken { get; private set; }

        public string OldRefreshToken { get; private set; }

        public ExchangeRefreshTokenCommand(
            Guid userId,
            string newRefreshToken,
            string oldRefreshToken)
        {
            UserId = userId;
            NewRefreshToken = newRefreshToken;
            OldRefreshToken = oldRefreshToken;
        }
    }
}
