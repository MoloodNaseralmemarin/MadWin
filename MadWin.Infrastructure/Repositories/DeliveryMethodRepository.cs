using MadWin.Core.Entities.CommissionRates;
using MadWin.Core.Entities.DeliveryMethods;
using MadWin.Core.Interfaces;
using MadWin.Core.Lookups.CommissionRates;
using MadWin.Core.Lookups.DeliveryMethods;
using MadWin.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace MadWin.Infrastructure.Repositories
{
    public class DeliveryMethodRepository : GenericRepository<DeliveryMethod>, IDeliveryMethodRepository
    {
        private readonly MadWinDBContext _context;
        public DeliveryMethodRepository(MadWinDBContext context):base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DeliveryMethodInfoLookup>> GetDeliveryMethodInfoAsync()
        {
            var items = await _context.DeliveryMethods
                .AsNoTracking()
                .ToListAsync();

            if (items == null || !items.Any())
                return Enumerable.Empty<DeliveryMethodInfoLookup>();

            return items.Select(item => new DeliveryMethodInfoLookup
            {
                Name = item.Name,
                Cost = item.Cost,
                DeliveryId=item.Id
            });
        }

        public async Task<decimal> GetDeliveryMethodCostByIdAsync(int deliveryId)
        {
            var cost = await _context.DeliveryMethods
                .AsNoTracking()
                .Where(x => x.Id == deliveryId)
                .Select(x => x.Cost)
                .FirstOrDefaultAsync();

            return cost;
        }

        public async Task<DeliveryMethodInfoLookup> GetDeliveryMethodInfoAsync(int deliveryId)
        {
            var item = await _context.DeliveryMethods
    .AsNoTracking()
    .FirstOrDefaultAsync(x => x.Id == deliveryId);

            if (item == null)
                return null;

            return new DeliveryMethodInfoLookup
            {
                DeliveryId = item.Id,
                Cost = item.Cost,
                Name=item.Name
            };


        }

    }
}
