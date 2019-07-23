using System.Threading.Tasks;
using Auth.Core.Repositry;

namespace Auth.Core.Services.Interfaces
{
    public interface IJwtFactory
    {
        Task<AccessToken> GenerateEncodedToken(string id, string userName);
    }
}
