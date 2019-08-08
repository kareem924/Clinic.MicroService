using System.Threading.Tasks;
using Security.Core.Dto;

namespace Security.Core.Interfaces
{
    public interface IJwtFactory
    {
        Task<AccessToken> GenerateEncodedToken(string id, string userName);
    }
}
