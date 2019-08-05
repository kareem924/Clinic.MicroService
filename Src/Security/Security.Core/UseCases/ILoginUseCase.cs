using System.Threading.Tasks;
using Security.Core.Dto;

namespace Security.Core.UseCases
{
    public interface ILoginUseCase
    {
        Task<LoginResponse> Handle(LoginDto loginRequest);
    }
}
