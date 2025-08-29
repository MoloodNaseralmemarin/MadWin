using MadWin.Application.DTOs.Orders;
using MadWin.Core.DTOs.Calculations;
using MadWin.Core.DTOs.Orders;
using MadWin.Core.Entities.Orders;


namespace MadWin.Application.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<CurtainComponentProductGroupLookup>> GetCalculationAsync(int categoryId, int subCategoryId);
        Task<int> CreateOrderInitialAsync(CreateOrderInitialDto dto, int userId,decimal basePrice);

        Task UpdatePriceAndCommissionAsync(int orderId, decimal basePrice, int commissionFee, int commissionId);

        Task UpdatePriceAndDeliveryAsync(int deliveryId,int orderId);

        Task<OrderSummaryDto> GetOrderSummaryByOrderIdAsync(int orderId);
        Task<IEnumerable<OrderSummaryDto>> GetOrderSummaryByUserIdAsync(int userId);

        Task UpdateIsFinalyOrderAsync(Order order);
        Task<Order> GetOrderByOrderIdAsync(int orderId);

        Task<int> CountOrders();

        Task<PagedResult<OrderSummaryDto>> GetOrderSummaryAsync(OrderFilterParameters filter);

    }
}
