using MadWin.Core.Entities.DeliveryMethods;
using MadWin.Core.Lookups.CommissionRates;
using MadWin.Core.Lookups.DeliveryMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Core.Interfaces
{
    public interface IDeliveryMethodRepository:IGenericRepository<DeliveryMethod>
    {
        Task<IEnumerable<DeliveryMethodInfoLookup>> GetDeliveryMethodInfoAsync();

        Task<decimal> GetDeliveryMethodCostByIdAsync(int deliveryId);

        Task<DeliveryMethodInfoLookup> GetDeliveryMethodInfoAsync(int deliveryId);

    }
}
