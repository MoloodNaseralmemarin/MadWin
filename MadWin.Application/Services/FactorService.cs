using MadWin.Core.Entities.Factors;
using MadWin.Core.Entities.Orders;
using MadWin.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace MadWin.Application.Services
{
    public class FactorService : IFactorService
    {
        private readonly IFactorRepository _factorRepository;
        private readonly IFactorDetailRepository _factorDetailRepository;
        public ILogger<IFactorService> _logger;


        public FactorService(IFactorRepository factorRepository,IFactorDetailRepository factorDetailRepository, ILogger<IFactorService> logger)
        {
            _factorRepository = factorRepository;
            _factorDetailRepository = factorDetailRepository;
            _logger = logger;
        }

        public async Task<int> AddFactorAsync(int userId, int productId, int count)
        {
            // پیدا کردن فاکتور باز کاربر (پرداخت نشده)
            var factor = await _factorRepository.GetOpenFactorByUserIdAsync(userId);

            if (factor == null)
            {
                // اگر فاکتور باز نداشت → یک فاکتور جدید بساز
                factor = await _factorRepository.AddFactorAsync(userId);
            }

            // بررسی کنیم آیا این محصول توی فاکتور باز هست یا نه
            var factorDetail = await _factorDetailRepository.GetFactorDetailByProductIdAsync(factor.Id, productId);

            if (factorDetail == null)
            {
                // اگر محصول وجود نداشت → رکورد جدید اضافه کن
                await _factorDetailRepository.AddFactorDetailAsync(factor.Id, count, productId);
            }
            else
            {
                // اگر محصول وجود داشت → فقط تعدادش رو زیاد کن
                factorDetail.Quantity += count;
                _factorDetailRepository.Update(factorDetail);
                await _factorDetailRepository.SaveChangesAsync();
            }

            // آپدیت مجموع فاکتور
            await _factorRepository.UpdateFactorSum(factor.Id);

            return factor.Id;
        }

        public async Task<Factor> GetFactorByFactorIdAsync(int factorId)
        {
            return await _factorRepository.GetFactorByFactorIdAsync(factorId);
        }

        public async Task UpdateIsFinalyFactorAsync(Factor factor)
        {
            factor.IsFinaly = true;
            factor.TotalAmount = factor.TotalPrice + factor.DeliveryMethodAmount - factor.DisTotal;
            _factorRepository.Update(factor);
            await _factorRepository.SaveChangesAsync();
        }

        public async Task UpdatePriceAndDeliveryAsync(int deliveryId, int factorId)
        {
            try
            {
                await _factorRepository.UpdatePriceAndDeliveryAsync(deliveryId, factorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating factor {FactorId} in OrderService", factorId);
                throw;
            }
        }


        public async Task<int> CreateOrGetOpenFactorAsync(int userId)
        {
            var factor = await _factorRepository.GetOpenFactorByUserIdAsync(userId);
            if (factor == null)
            {
                factor = await _factorRepository.AddFactorAsync(userId);
            }
            return factor.Id;
        }

        public async Task AddOrUpdateFactorDetailAsync(int factorId, int productId, int count)
        {
            var factorDetail = await _factorDetailRepository.GetFactorDetailByProductIdAsync(factorId, productId);

            if (factorDetail == null)
            {
                await _factorDetailRepository.AddFactorDetailAsync(factorId, count, productId);
            }
            else
            {
                factorDetail.Quantity += count;
                _factorDetailRepository.Update(factorDetail);
                await _factorDetailRepository.SaveChangesAsync();
            }
        }

        public async Task UpdateFactorSumAsync(int factorId)
        {
            await _factorRepository.UpdateFactorSum(factorId);
        }

        public decimal GetSubtotal(int factorId)
        {
            return _factorDetailRepository.GetSubtotalByFactorId(factorId);
        }

    }
}