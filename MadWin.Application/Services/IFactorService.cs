using MadWin.Core.DTOs.Factors;
using MadWin.Core.DTOs.Orders;
using MadWin.Core.Entities.Factors;
using MadWin.Core.Entities.Orders;

namespace MadWin.Application.Services
{
    public interface IFactorService
    {
        Task<int> AddFactorAsync(int userId, int productId, int count);

        Task<Factor> GetFactorByFactorIdAsync(int factorId);

        Task UpdatePriceAndDeliveryAsync(int deliveryId, int factorId);
        Task UpdateIsFinalyFactorAsync(Factor factor);

        Task<int> CreateOrGetOpenFactorAsync(int userId);
        Task AddOrUpdateFactorDetailAsync(int factorId, int productId, int count);
        Task UpdateFactorSumAsync(int factorId);
        decimal GetSubtotal(int factorId);

    }
}


