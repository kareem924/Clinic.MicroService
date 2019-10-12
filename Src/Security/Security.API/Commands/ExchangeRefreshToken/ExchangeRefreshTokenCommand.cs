using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.CQRS;

namespace Security.API.Commands.ExchangeRefreshToken
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
