using MadWin.Core.Entities.Discounts;
using MadWin.Core.Entities.Users;
using MadWin.Core.Interfaces;
using MadWin.Core.Lookups.Discounts;
using MadWin.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MadWin.Infrastructure.Repositories
{
    public class DiscountRepository : GenericRepository<Discount>, IDiscountRepository
    {
        private readonly MadWinDBContext _context;
        public DiscountRepository(MadWinDBContext context) : base(context)
        {
            _context = context;
        }
        public async Task<DiscountInfoLookup> GetDisCountForOrderInfoAsync(string code)
        {
            var discount= await _context.Set<Discount>()
                .AsNoTracking()
                .Where(dc => dc.DiscountCode == code && dc.Item == "O")
                .Select(dc => new DiscountInfoLookup
                {
                    DiscountCode = dc.DiscountCode,
                    DiscountId = dc.Id,
                    DiscountPercent = dc.Percentage,
                    EndDate = dc.ExpiryDate,
                    StartDate = dc.StartDate,
                    UseableCount = dc.UseableCount
                })
                .FirstOrDefaultAsync();
            if(discount == null ) 
                return null;
            return discount;
        }

        public async Task<DiscountInfoLookup> GetDiscountInfoByDiscountIdAsync(int discountId)
        {
            var discount = await _context.Set<Discount>()
                .AsNoTracking()
                .Where(dc => dc.Id == discountId)
                .Select(dc => new DiscountInfoLookup
                {
                    DiscountCode = dc.DiscountCode,
                    DiscountId = dc.Id,
                    DiscountPercent = dc.Percentage,
                })
                .FirstOrDefaultAsync();
            if (discount == null)
                return null;
            return discount;
        }

        public async Task<DiscountInfoLookup> GetDisCountForFactorInfoAsync(string code)
        {
            var discount = await _context.Set<Discount>()
                .AsNoTracking()
                .Where(dc => dc.DiscountCode == code && dc.Item == "F")
                .Select(dc => new DiscountInfoLookup
                {
                    DiscountCode = dc.DiscountCode,
                    DiscountId = dc.Id,
                    DiscountPercent = dc.Percentage,
                    EndDate = dc.ExpiryDate,
                    StartDate = dc.StartDate,
                    UseableCount = dc.UseableCount
                })
                .FirstOrDefaultAsync();
            if (discount == null)
                return null;
            return discount;
        }

        public async Task<bool> HasUserUsedDiscountAsync(int userId, int discountId, int orderId)
        {
            return await _context.Set<UserDiscountCode>()
                .AnyAsync(x => x.UserId == userId
                            && x.DisCountId == discountId
                            && x.OrderId == orderId);
        }

        public async Task<bool> IsExistDisCountCode(string discountcode)
        {
            return await _context.Set<Discount>()
             .AnyAsync(dc => dc.DiscountCode == discountcode);
        }
    }
}
