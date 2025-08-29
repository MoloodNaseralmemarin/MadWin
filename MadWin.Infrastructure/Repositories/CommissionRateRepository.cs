using MadWin.Core.Entities.CommissionRates;
using MadWin.Core.Entities.Orders;
using MadWin.Core.Interfaces;
using MadWin.Core.Lookups.CommissionRates;
using MadWin.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Infrastructure.Repositories
{
    public class CommissionRateRepository : GenericRepository<CommissionRate>, ICommissionRateRepository
    {
        private readonly MadWinDBContext _context;
        public CommissionRateRepository(MadWinDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<CommissionInfoLookup> GetCommissionInfoAsync(int partCount, bool isEqualParts)
        {
            var item = await _context.CommissionRates
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PartCount == partCount && x.IsEqualParts == isEqualParts);

            if (item == null)
                return null;

            return new CommissionInfoLookup
            {
                CommissionRateId = item.Id,
                CommissionPercent=item.CommissionPercent
            };
        }


    }
}
