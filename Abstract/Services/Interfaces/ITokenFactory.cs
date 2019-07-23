namespace Auth.Core.Services.Interfaces
{
    public interface ITokenFactory
    {
        string GenerateToken(int size = 32);
    }
}
