using MadWin.Core.Entities.CurtainComponents;
using MadWin.Core.Interfaces;
using MadWin.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Infrastructure.Repositories
{
    public class CurtainComponentRepository:GenericRepository<CurtainComponent>,ICurtainComponentRepository
    {
        private readonly MadWinDBContext _context;
        public CurtainComponentRepository(MadWinDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<decimal> GetPriceByIdAsync(int id)
        {
            return await _context.CurtainComponents
                                 .Where(x => x.Id == id)
                                 .Select(x =>x.Cost)
                                 .FirstOrDefaultAsync();
        }
    }
}
