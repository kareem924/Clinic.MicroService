using System;
using System.Threading.Tasks;
using Security.Core.Dto;

namespace Security.Core.Services
{
    public interface IJwtFactory
    {
        Task<AccessToken> GenerateEncodedToken(string id, string userName);
    }
}
