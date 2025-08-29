using MadWin.Core.DTOs.Factors;
using MadWin.Core.DTOs.Orders;
using MadWin.Core.Entities.Factors;
using MadWin.Core.Entities.Orders;
using MadWin.Core.Entities.Users;
using MadWin.Core.Interfaces;
using MadWin.Core.Lookups.Factors;
using MadWin.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MadWin.Infrastructure.Repositories
{
    public class FactorRepository : GenericRepository<Factor>, IFactorRepository
    {
        private readonly MadWinDBContext _context;  
        private readonly IFactorDetailRepository _factorDetailRepository;
        private readonly IDeliveryMethodRepository _deliveryMethodRepository;
        public FactorRepository(MadWinDBContext context, IFactorDetailRepository factorDetailRepository, IDeliveryMethodRepository deliveryMethodRepository) : base(context)
        {
            _context = context;
            _factorDetailRepository = factorDetailRepository;
            _deliveryMethodRepository = deliveryMethodRepository;
        }
        public async Task<bool> IsExistOpenFactorAsync(int userId, bool isFinaly)
        {
            return await _context.Set<Factor>().AnyAsync(u => u.UserId == userId && u.IsFinaly==isFinaly);
        }

        public async Task<Factor> AddFactorAsync(int userId)
        {
            Factor factor=new Factor();
            factor.UserId = userId;
            factor.IsFinaly = false;
            factor.FactorSum = 0;
          //  factor.
            await AddAsync(factor);
            await _context.SaveChangesAsync();
            return factor;
        }
        public async Task UpdateFactorSum(int factorId)
        {
            var factor = await GetByIdAsync(factorId);
            var factorSum =await _factorDetailRepository.FactorSum(factorId);
            factor.FactorSum = factorSum;
            Update(factor);    
            await SaveChangesAsync();
        }

        public async Task<Factor> GetFactorAsync(int userId)
        {
            var factor = await _context.Factors
               .FirstOrDefaultAsync(o => o.UserId == userId && !o.IsFinaly);
            return factor;
        }

        public async Task<Factor> GetFactorByUserIdAsync(int userId)
        {
            var factor =await _context.Set<Factor>()
                            .FirstOrDefaultAsync(o => o.UserId == userId && !o.IsFinaly);
            return factor;

        }

        public async Task<FactorInfoLookup> GetFactorInfoByFactorIdAsync(int factorId)
        {
            var factor = await _context.Set<Factor>()
                .Where(o => o.Id == factorId)
                .Select(o => new FactorInfoLookup
                {
                    FactorId = o.Id,
                    Price = o.FactorSum
                }).FirstOrDefaultAsync();

            if (factor == null)
                return null;
            return factor;
        }



        public async Task<Factor> GetFactorByFactorIdAsync(int factorId)
        {
            return await _context.Set<Factor>().AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == factorId);
        }

        public async Task UpdatePriceAndDeliveryAsync(int deliveryId, int factorId)
        {
            var deliveryMethod = await _deliveryMethodRepository.GetDeliveryMethodInfoAsync(deliveryId);
            var factor = await GetByIdAsync(factorId);
            factor.DeliveryMethodId = deliveryMethod.DeliveryId;
            factor.DeliveryMethodAmount = deliveryMethod.Cost;
            _context.Set<Factor>().Update(factor);
            await _context.SaveChangesAsync();
        }
    }
}
