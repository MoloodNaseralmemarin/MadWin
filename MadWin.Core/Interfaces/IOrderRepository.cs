using MadWin.Core.DTOs.Orders;
using MadWin.Core.Entities.Orders;
using MadWin.Core.Lookups.Orders;

namespace MadWin.Core.Interfaces
{
    public interface IOrderRepository:IGenericRepository<Order>
    {
        Task<int> CreateOrderInitialAsync(Order order);

        Task UpdatePriceAndCommissionAsync(int orderId, decimal basePrice, int commissionFee, int commissionId);

        Task UpdatePriceAndDeliveryAsync(int deliveryId,int orderId);

        Task<OrderSummaryDto> GetOrderSummaryByOrderIdAsync(int orderId);

        Task<IEnumerable<OrderSummaryDto>> GetOrderSummaryByUserIdAsync(int userId);
        Task<Order> GetOrderByOrderIdAsync(int orderId);

        Task<OrderInfoLookup> GetOrderInfoByOrderIdAsync(int orderId);
        Task<int> CountOrders();
        Task<PagedResult<OrderSummaryDto>> GetOrderSummaryAsync(OrderFilterParameters filter);

        Task<PagedResult<OrderSummaryDto>> GetTodayOrdersAsync(int PageNumber = 1, int PageSize = 10);

        Task SoftDeleteFromOrderAsync(IEnumerable<int> orderIds);
    }
}
