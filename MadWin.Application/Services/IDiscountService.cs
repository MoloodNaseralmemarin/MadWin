using MadWin.Application.DTOs.Discounts;
using MadWin.Core.DTOs.Orders;
using MadWin.Core.Entities.Users;
using MadWin.Core.Interfaces;

namespace MadWin.Application.Services
{
    public interface IDiscountService
    {
        Task<DiscountUseType> UseDiscountAsync(int orderId, string code, int userId);

        Task<DiscountResultDto> ApplyDiscountAsync(int orderId, string discountCode);

        Task<DiscountResultDto> ApplyDiscountForFactorAsync(int factorId, string discountCode);
        Task<DiscountUseType> UseDiscountForFactorAsync(int factorId, string code, int userId);
    }
}
        