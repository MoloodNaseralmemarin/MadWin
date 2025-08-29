using MadWin.Core.DTOs.Factors;
using MadWin.Core.DTOs.Orders;
using MadWin.Core.Lookups.DeliveryMethods;

namespace Shop2City.WebHost.ViewModels.Factors
{
    public class FactorSummaryViewModel
    {
        public FactorSummaryDto FactorSummary { get; set; }
        public IEnumerable<DeliveryMethodInfoLookup> DeliveryMethods { get; set; }
    }

}
