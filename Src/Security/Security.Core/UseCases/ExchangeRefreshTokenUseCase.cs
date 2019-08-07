using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.General.Dto.Result;
using Security.Core.Dto;
using Security.Core.Repositories;
using Security.Core.Services;
using Security.Core.Specification;

namespace Security.Core.UseCases
{
    public sealed class ExchangeRefreshTokenUseCase : IExchangeRefreshTokenUseCase
    {

        private readonly IUserRepository _userRepository;
        private readonly IJwtFactory _jwtFactory;
        private readonly ITokenFactory _tokenFactory;
        private readonly IJwtTokenValidator _jwtTokenValidator;

        public ExchangeRefreshTokenUseCase(
            IUserRepository userRepository,
            IJwtFactory jwtFactory,
            ITokenFactory tokenFactory,
            IJwtTokenValidator jwtTokenValidator)
        {

            _userRepository = userRepository;
            _jwtFactory = jwtFactory;
            _tokenFactory = tokenFactory;
            _jwtTokenValidator = jwtTokenValidator;
        }

        public async Task<ExchangeRefreshTokenResponse> Handle(ExchangeRefreshTokenDto message)
        {
            var claimPrincipalFromToken = _jwtTokenValidator.GetPrincipalFromToken(message.AccessToken, message.SigningKey);

            if (claimPrincipalFromToken != null)
            {
                var id = claimPrincipalFromToken.Claims.First(c => c.Type == "id");
                var user = await _userRepository.GetAsync(id.Value);
                if (user.HasValidRefreshToken(message.RefreshToken))
                {
                    var jwtToken = await _jwtFactory.GenerateEncodedToken(user.Id.ToString(), user.UserName);
                    var refreshToken = _tokenFactory.GenerateToken();
                    user.RemoveRefreshToken(message.RefreshToken); 
                    user.AddRefreshToken(refreshToken, user.Id, "");
                    await _userRepository.UpdateAsync(user, user.Id);
                    return new ExchangeRefreshTokenResponse(jwtToken, refreshToken, success: true);
                }
            }
            return new ExchangeRefreshTokenResponse(success: false, errors: new[] { new Error("Invalid token.") });
        }


    }
}
