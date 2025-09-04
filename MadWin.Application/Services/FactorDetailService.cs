using MadWin.Core.DTOs.Factors;
using MadWin.Core.Entities.Factors;
using MadWin.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Application.Services
{
    public class FactorDetailService : IFactorDetailService
    {
        private readonly IFactorDetailRepository _factorDetailRepository;
        private readonly IFactorRepository _factorRepository;
        public FactorDetailService(IFactorDetailRepository factorDetailRepository, IFactorRepository factorRepository)
        {
            _factorDetailRepository = factorDetailRepository;
            _factorRepository = factorRepository;
        }

        public async Task<List<FactorDetail>> GetAllFactorDetailByFactorIdAsync(int factorId)
        {
            return await _factorDetailRepository.GetAllFactorDetailByFactorIdAsync(factorId);
        }

        public async Task<FactorSummaryDto> GetFactorSummaryByFactorIdAsync(int factorId)
        {
            return await _factorDetailRepository.GetFactorSummaryByFactorIdAsync(factorId);
        }
        public async Task SoftDeleteAsync(IEnumerable<int> factorDetailIds)
        {
            if (factorDetailIds == null || !factorDetailIds.Any())
                return;

            var allFactorDetails = await _factorDetailRepository.GetAllAsync();
            var factorDetailsToDelete = allFactorDetails.Where(fd => factorDetailIds.Contains(fd.Id) && !fd.IsDelete).ToList();

            if (!factorDetailsToDelete.Any())
                return;

            // گروه‌بندی ریز فاکتورهای حذف شده بر اساس فاکتور مربوطه
            var factorIds = factorDetailsToDelete.Select(fd => fd.FactorId).Distinct();

            foreach (var detail in factorDetailsToDelete)
            {
                detail.IsDelete = true;
                detail.LastUpdateDate = DateTime.Now;
                detail.Description = "توسط کاربر حذف شده است.";
                _factorDetailRepository.Update(detail);
            }

            await _factorDetailRepository.SaveChangesAsync();

            // حالا قیمت فاکتورها رو بروزرسانی کن
            var allFactors = await _factorRepository.GetAllAsync();

            foreach (var factorId in factorIds)
            {
                // جمع قیمت ریز فاکتورهای حذف نشده برای هر فاکتور
                var sumPrice = allFactorDetails
                    .Where(fd => fd.FactorId == factorId && !fd.IsDelete)
                    .Sum(fd => fd.Price);

                var factor = allFactors.FirstOrDefault(f => f.Id == factorId);
                if (factor != null)
                {
                    factor.TotalPrice = sumPrice;
                    factor.LastUpdateDate = DateTime.Now;
                    factor.Description = "تغییر قیمت رخ داد چون ایتمی توسط کاربر حذف شده است";
                    _factorRepository.Update(factor);
                }
            }

            await _factorRepository.SaveChangesAsync();
        }

    }
}
