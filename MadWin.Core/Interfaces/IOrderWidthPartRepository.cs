using MadWin.Core.Entities.CommissionRates;
using MadWin.Core.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Core.Interfaces
{
    public interface IOrderWidthPartRepository: IGenericRepository<OrderWidthPart>
    {
        string GetStringAsync(int orderId);
    }
}
