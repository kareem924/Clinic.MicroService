namespace Security.API.Dto
{
    public class TokenRequestDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool RemeberMe { get; set; }

        public string DeviceName { get; set; }
    }
}
