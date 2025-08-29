using MadWin.Core.Entities.Factors;
using MadWin.Core.Lookups.Factors;

namespace MadWin.Core.Interfaces
{
    public interface IFactorRepository: IGenericRepository<Factor>
    {
        Task<Factor> AddFactorAsync(int userId);
        Task<bool> IsExistOpenFactorAsync(int userId, bool isFinaly);

        Task UpdateFactorSum(int factorId);
        Task<Factor> GetFactorByUserIdAsync(int userId);
        Task<Factor> GetFactorByFactorIdAsync(int factorId);
        Task<FactorInfoLookup> GetFactorInfoByFactorIdAsync(int factorId);

        Task UpdatePriceAndDeliveryAsync(int deliveryId, int factorId);
    }
}
