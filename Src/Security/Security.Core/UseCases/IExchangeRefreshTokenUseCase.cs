using System.Threading.Tasks;
using Security.Core.Dto;

namespace Security.Core.UseCases
{
    public interface IExchangeRefreshTokenUseCase
    {
        Task<ExchangeRefreshTokenResponse> Handle(ExchangeRefreshTokenDto message);
    }
}