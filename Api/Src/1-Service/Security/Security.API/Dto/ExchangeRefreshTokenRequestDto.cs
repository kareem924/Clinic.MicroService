namespace Security.API.Dto
{
    public class ExchangeRefreshTokenRequestDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
