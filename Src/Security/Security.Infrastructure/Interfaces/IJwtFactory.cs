using System.Threading.Tasks;
using Security.Core.Dto;

namespace Security.Infrastructure.Interfaces
{
    public interface IJwtFactory
    {
        Task<AccessTokenDto> GenerateEncodedToken(string id, string userName);
    }
}
