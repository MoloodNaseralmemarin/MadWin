using MadWin.Core.Entities.Discounts;
using MadWin.Core.Lookups.Discounts;

namespace MadWin.Core.Interfaces
{
    public interface IDiscountRepository:IGenericRepository<Discount>
    {
        Task<DiscountInfoLookup> GetDisCountForOrderInfoAsync(string code);
        Task<DiscountInfoLookup> GetDisCountForFactorInfoAsync(string code);

        Task<DiscountInfoLookup> GetDiscountInfoByDiscountIdAsync(int discountId);

        Task<bool> HasUserUsedDiscountAsync(int userId, int discountId, int orderId);

        Task<bool> IsExistDisCountCode(string discountcode);
    }
}
