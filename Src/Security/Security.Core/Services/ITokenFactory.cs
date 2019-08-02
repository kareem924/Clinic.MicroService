
namespace Security.Core.Services
{
    public interface ITokenFactory
    {
        string GenerateToken(int size= 32);
    }
}
