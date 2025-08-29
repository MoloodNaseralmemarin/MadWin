using MadWin.Core.DTOs.Orders;
using MadWin.Core.Entities.CommissionRates;
using MadWin.Core.Entities.Orders;
using MadWin.Core.Interfaces;
using MadWin.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Infrastructure.Repositories
{
    public class OrderWidthPartRepository : GenericRepository<OrderWidthPart>, IOrderWidthPartRepository
    {
        private readonly MadWinDBContext _context;
        public OrderWidthPartRepository(MadWinDBContext context) : base(context)
        {
            _context = context;
        }

        public string GetStringAsync(int orderId)
        {
            var s="";
            var widthParts =  _context.Set<OrderWidthPart>()
            .Where(w => w.OrderId == orderId).ToList();
            foreach(var item in widthParts)
            {
                s = s + item.WidthValue + "-";
            }
            return  s;
       
        }
    }
}
