using MadWin.Application.DTOs.Discounts;
using MadWin.Core.DTOs.DisCounts;
using MadWin.Core.DTOs.Orders;
using MadWin.Core.Entities.Users;
using MadWin.Core.Interfaces;

namespace MadWin.Application.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IUserDiscountCodeRepository _userDiscountCodeRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IFactorRepository _factorRepository;

        public DiscountService(IDiscountRepository discountRepository,IUserDiscountCodeRepository userDiscountCodeRepository, IOrderRepository orderRepository, IFactorRepository factorRepository)
        {
            _discountRepository = discountRepository;
            _userDiscountCodeRepository = userDiscountCodeRepository;
            _orderRepository = orderRepository;
            _factorRepository = factorRepository;
        }

        public async Task<DiscountResultDto> ApplyDiscountAsync(int orderId, string discountCode)
        {
            var result = new DiscountResultDto();

            //var order = await _orderRepository.GetOrderInfoByOrderIdAsync(orderId);
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                return null;
            }

            //ببین میتونی در اینده بکنیتش ی سروریس
            var discount = await _discountRepository.GetDisCountForOrderInfoAsync(discountCode);
            result.OriginalPrice = order.PriceWithFee;
            result.DiscountPercentage = discount.DiscountPercent;

            result.DiscountAmount = (result.OriginalPrice * discount.DiscountPercent) / 100;
            result.FinalPrice = order.PriceWithFee - result.DiscountAmount;

            #region update discount
            order.DiscountId = discount.DiscountId;
            order.DisPercent = discount.DiscountPercent;
            order.DisTotal = result.DiscountAmount;
            await _orderRepository.SaveChangesAsync();
            #endregion

            return result;
        }
        public async Task<DiscountResultDto> ApplyDiscountForFactorAsync(int factorId, string discountCode)
        {
            var result = new DiscountResultDto();

            var factor = await _factorRepository.GetFactorInfoByFactorIdAsync(factorId);
            if (factor == null)
            {
                return null;
            }

            var discount = await _discountRepository.GetDisCountForFactorInfoAsync(discountCode);
            result.OriginalPrice = factor.Price;
            result.DiscountPercentage = discount.DiscountPercent;

            result.DiscountAmount = (result.OriginalPrice * discount.DiscountPercent) / 100;
            result.FinalPrice = factor.Price - result.DiscountAmount;

            return result;
        }
        public async Task<DiscountUseType> UseDiscountAsync(int orderId, string code, int userId)
        {
            var discount = await _discountRepository.GetDisCountForOrderInfoAsync(code);

            if (discount == null)
                return DiscountUseType.NotFound;

            if (discount.EndDate <= DateTime.Now)
                return DiscountUseType.ExpirationDate;

            if (discount.UseableCount < 1)
                return DiscountUseType.Finished;

            if (await _discountRepository.HasUserUsedDiscountAsync(userId, discount.DiscountId, orderId))
                return DiscountUseType.UserUsed;

            await _userDiscountCodeRepository.AddAsync(new UserDiscountCode
            {
                UserId = userId,
                DisCountId = discount.DiscountId,
                OrderId = orderId
            });
            await _userDiscountCodeRepository.SaveChangesAsync();
            return DiscountUseType.Success;
        }

        public async Task<DiscountUseType> UseDiscountForFactorAsync(int factorId, string code, int userId)
        {
            var discount = await _discountRepository.GetDisCountForFactorInfoAsync(code);

            if (discount == null)
                return DiscountUseType.NotFound;

            if (discount.EndDate <= DateTime.Now)
                return DiscountUseType.ExpirationDate;

            if (discount.UseableCount < 1)
                return DiscountUseType.Finished;

            if (await _discountRepository.HasUserUsedDiscountAsync(userId, discount.DiscountId, factorId))
                return DiscountUseType.UserUsed;

            await _userDiscountCodeRepository.AddAsync(new UserDiscountCode
            {
                UserId = userId,
                DisCountId = discount.DiscountId,
                FactorId = factorId
            });
            await _userDiscountCodeRepository.SaveChangesAsync();
            return DiscountUseType.Success;
        }

    }
}
