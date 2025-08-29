using MadWin.Core.DTOs.Orders;
using MadWin.Core.Lookups.DeliveryMethods;

namespace Shop2City.WebHost.ViewModels.Orders
{
    public class OrderSummaryViewModel
    {
        public OrderSummaryDto OrderSummary { get; set; }
        public IEnumerable<DeliveryMethodInfoLookup> DeliveryMethods { get; set; }
    }

}
