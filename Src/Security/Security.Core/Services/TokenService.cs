using System.Linq;
using System.Threading.Tasks;
using Common.General.Dto.Result;
using Security.Core.Dto;
using Security.Core.Interfaces;
using Security.Core.Repositories;

namespace Security.Core.Services
{
    public sealed class TokenService : ITokenService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtFactory _jwtFactory;
        private readonly ITokenFactory _tokenFactory;
        private readonly IJwtTokenValidator _jwtTokenValidator;
        public TokenService(
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

      

        public async Task<ExchangeRefreshTokenResponse> GenerateRefreshToken(ExchangeRefreshTokenDto message)
        {

            var claimPrincipalFromToken = _jwtTokenValidator.GetPrincipalFromToken(message.AccessToken, message.SigningKey);

            if (claimPrincipalFromToken != null)
            {
                var id = claimPrincipalFromToken.Claims.First(c => c.Type == "id");
                var user = await _userRepository.GetByIdAsync(id.Value);
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

        public async Task<LoginResponse> GetToken(LoginDto loginRequest)
        {
            if (!string.IsNullOrEmpty(loginRequest.UserName) && !string.IsNullOrEmpty(loginRequest.Password))
            {
                // ensure we have a user with the given user name
                var user = await _userRepository.FindByName(loginRequest.UserName);
                if (user != null)
                {
                    // validate password
                    if (await _userRepository.CheckPassword(user, loginRequest.Password))
                    {
                        // generate refresh token
                        var refreshToken = _tokenFactory.GenerateToken();
                        user.AddRefreshToken(refreshToken, user.Id, loginRequest.RemoteIpAddress);
                        await _userRepository.UpdateAsync(user, user.Id);

                        // generate access token
                        return new LoginResponse(
                            await _jwtFactory.GenerateEncodedToken(user.Id.ToString(), user.UserName),
                            refreshToken,
                            success: true);
                    }
                }
            }
            return new LoginResponse(new[] { new Error("Invalid Password Or Email") }, false);
        }
    }
}
