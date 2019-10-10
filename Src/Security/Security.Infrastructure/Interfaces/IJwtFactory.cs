using System.Threading.Tasks;
using Security.Core.Dto;

namespace Security.Infrastructure.Interfaces
{
    internal interface IJwtFactory
    {
        Task<AccessTokenDto> GenerateEncodedToken(string id, string userName);
    }
}
