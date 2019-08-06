using System.Threading.Tasks;
using Common.General.Dto.Result;
using Security.Core.Dto;
using Security.Core.Repositories;
using Security.Core.Services;

namespace Security.Core.UseCases
{
    public sealed class LoginUseCase : ILoginUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtFactory _jwtFactory;
        private readonly ITokenFactory _tokenFactory;

        public LoginUseCase(IUserRepository userRepository, IJwtFactory jwtFactory, ITokenFactory tokenFactory)
        {
            _userRepository = userRepository;
            _jwtFactory = jwtFactory;
            _tokenFactory = tokenFactory;
        }

        public async Task<LoginResponse> Handle(LoginDto loginRequest)
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
                        await _userRepository.UpdateAsync(user,user.Id);

                        // generate access token
                        return new LoginResponse(
                            await _jwtFactory.GenerateEncodedToken(user.Id.ToString(), user.UserName),
                            refreshToken,
                            success: true);
                    }
                }
            }
            return new LoginResponse(new [] {new Error("Invalid Password Or Email")}, false);
        }
    }
}
