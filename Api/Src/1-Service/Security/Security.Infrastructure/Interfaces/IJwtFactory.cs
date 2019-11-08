using System.Threading.Tasks;
using Security.Core.Dto;
using Security.Core.Entities;

namespace Security.Infrastructure.Interfaces
{
    public interface IJwtFactory
    {
        Task<AccessTokenDto> GenerateEncodedToken(User user);
    }
}
