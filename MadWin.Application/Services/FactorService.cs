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

            //اول چک کنیم ببینیم سفارش باز داره یعنی پرداخت نهایی نکرده است
            //اگر فاکتور باز داشت باید به اون فاکتور اضافه بشه
            //اگر فاکتور باز داشت باید به اون فاکتور اضافه بشه

            var isOpenFactor =await _factorRepository.IsExistOpenFactorAsync(userId,false);
            var factor=await _factorRepository.GetFactorByUserIdAsync(userId);

            //اگر سفارش خالی بود
            if (!isOpenFactor)
            {
             factor= await _factorRepository.AddFactorAsync(userId);
                await _factorDetailRepository.AddFactorDetailAsync(factor.Id, count, productId);
            }
            else
            {

                //کاربر فاکتور باز داره
                //الان باید چک کنیم از کالای انتخاب شده توی فاکتورش هست یا نه 

                var isExistFactorDetail = await _factorDetailRepository.IsExistFactorDetailAsync(factor.Id);
                var factorDetail = await _factorDetailRepository.GetFactorByfactorIdAsync(factor.Id);
                // اگر همچنین کالایی وجود نداشت
                if (isExistFactorDetail)
                {
                    factorDetail = await _factorDetailRepository.AddFactorDetailAsync(factor.Id, count, productId);

                }
                //اینجا محصول وجود نداشت توی فاکتور باز
                else
                {
                    //یدونه به تعدادش اضافه کن
                    factorDetail.Quantity += 1;
                    _factorDetailRepository.Update(factorDetail);
                    await _factorDetailRepository.SaveChangesAsync();
                }
  

            }

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
            factor.TotalAmount = factor.FactorSum + factor.DeliveryMethodAmount - factor.DisTotal;
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
    }
}