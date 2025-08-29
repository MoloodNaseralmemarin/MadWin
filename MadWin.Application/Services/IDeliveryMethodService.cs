using MadWin.Core.Lookups.DeliveryMethods;

namespace MadWin.Application.Services
{
    public interface IDeliveryMethodService
    {
        Task<IEnumerable<DeliveryMethodInfoLookup>> GetDeliveryMethodInfoAsync();
        Task<decimal> GetDeliveryMethodCostByIdAsync(int deliveryId);
    }
}
