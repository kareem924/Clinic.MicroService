namespace Security.Core.Dto
{
    public sealed class AccessTokenDto
    {
        public string Token { get; }
        public int ExpiresIn { get; }

        public AccessTokenDto(string token, int expiresIn)
        {
            Token = token;
            ExpiresIn = expiresIn;
        }
    }
}
