using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Security.API;
using Security.API.Dto;
using Xunit;

namespace FunctionalTest.Api
{
    public class AccountControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public AccountControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            Client = factory.CreateClient();
        }

        private HttpClient Client { get; }

        [Fact]
        public async Task GenerateToken_GivenValidRequest_ShouldTokenResponse()
        {
            var response = await Client.GetAsync("/api/account/token?UserName=demouser@microsoft.com&Password=Pass@word1");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<TokenResponseDto>(stringResponse);

            Assert.True(model.AccessToken != null);
        }

        [Fact]
        public async Task GenerateToken_GivenInvalidRequest_ShouldReturnBadRequest()
        {
            var response = await Client.GetAsync("/api/account/token?UserName=demouser&Password=Pass@word1");
            var badRequestStatusCode = response.StatusCode == HttpStatusCode.BadRequest;

            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<TokenResponseDto>(stringResponse);

            Assert.True(model.AccessToken == null);
            Assert.True(badRequestStatusCode);
        }

        [Fact]
        public async Task GenerateRefreshToken_GivenValidRefreshAccessToken_ShouldReturnTokenResponse()
        {
            var response = await Client.GetAsync("/api/account/token?UserName=demouser@microsoft.com&Password=Pass@word1");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<TokenResponseDto>(stringResponse);

            var refreshTokenResponse = await Client.GetAsync(
                $"/api/account/refresh-token?accessToken={model.AccessToken.Token}&refreshToken={model.RefreshToken}");
            refreshTokenResponse.EnsureSuccessStatusCode();
            var refreshTokenStringResponse = await refreshTokenResponse.Content.ReadAsStringAsync();
            var refreshTokenModel = JsonConvert.DeserializeObject<ExchangeRefreshTokenResponseDto>(refreshTokenStringResponse);
            Assert.True(refreshTokenModel.AccessToken != null);
        }

        [Fact]
        public async Task GenerateRefreshToken_GivenInvalidAccessToken_ShouldReturnTokenResponse()
        {
            var response = await Client.GetAsync("/api/account/token?UserName=demouser@microsoft.com&Password=Pass@word1");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<TokenResponseDto>(stringResponse);

            var refreshTokenResponse = await Client.GetAsync(
                $"/api/account/refresh-token?accessToken=112233&refreshToken={model.RefreshToken}");
            var badRequestStatusCode = refreshTokenResponse.StatusCode == HttpStatusCode.BadRequest;
            var refreshTokenStringResponse = await refreshTokenResponse.Content.ReadAsStringAsync();
            var refreshTokenModel = JsonConvert.DeserializeObject<ExchangeRefreshTokenResponseDto>(refreshTokenStringResponse);

            Assert.True(badRequestStatusCode);
            Assert.True(refreshTokenModel.AccessToken == null);
        }

        [Fact]
        public async Task GenerateRefreshToken_GivenInvalidRefreshToken_ShouldReturnTokenResponse()
        {
            var response = await Client.GetAsync("/api/account/token?UserName=demouser@microsoft.com&Password=Pass@word1");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<TokenResponseDto>(stringResponse);

            var refreshTokenResponse = await Client.GetAsync(
                $"/api/account/refresh-token?accessToken={model.AccessToken}&refreshToken=112233");
            var badRequestStatusCode = refreshTokenResponse.StatusCode == HttpStatusCode.BadRequest;
            var refreshTokenStringResponse = await refreshTokenResponse.Content.ReadAsStringAsync();
            var refreshTokenModel = JsonConvert.DeserializeObject<ExchangeRefreshTokenResponseDto>(refreshTokenStringResponse);

            Assert.True(badRequestStatusCode);
            Assert.True(refreshTokenModel.AccessToken == null);
        }
    }
}
