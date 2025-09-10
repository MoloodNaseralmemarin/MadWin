using MadWin.Application.DTOs.Orders;
using MadWin.Core.DTOs.Calculations;
using MadWin.Core.DTOs.Orders;
using MadWin.Core.Entities.Common;
using MadWin.Core.Entities.Orders;
using MadWin.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace MadWin.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderWidthPartRepository _orderWidthPartRepository;
        private readonly ICurtainComponentProductGroupRepository _curtainComponentProductGroupRepository;
        public OrderService(ILogger<OrderService> logger, IOrderRepository orderRepository, IOrderWidthPartRepository orderWidthPartRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
            _orderWidthPartRepository= orderWidthPartRepository;

        }
        public async Task<int> CreateOrderInitialAsync(CreateOrderInitialDto dto, int userId, decimal basePrice)
        {
            var order = new Order
            {
                UserId = userId,
                CategoryId = dto.CategoryId,
                SubCategoryId = dto.SubCategoryId,
                Height = dto.Height,
                Width = dto.Width,
                Count = dto.Count,
                BasePrice = basePrice,
                PartCount = dto.PartCount,
                IsEqualParts = dto.IsEqualParts
            };

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync(); // اینجا Id ساخته می‌شود

            if (dto.WidthParts != null && dto.WidthParts.Any())
            {
                foreach (var width in dto.WidthParts)
                {
                    var widthPart = new OrderWidthPart
                    {
                        OrderId = order.Id,
                        WidthValue = width
                    };
                    await _orderWidthPartRepository.AddAsync(widthPart);
                }
                await _orderWidthPartRepository.SaveChangesAsync();
            }

            return order.Id;
        }

        public async Task UpdatePriceAndCommissionAsync(int orderId, decimal basePrice, int commissionFee, int commissionId)
        {
            try
            {
                await _orderRepository.UpdatePriceAndCommissionAsync(orderId, basePrice, commissionFee, commissionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order {OrderId} in OrderService", orderId);
                throw;
            }
        }
        public async Task<IEnumerable<CurtainComponentProductGroupLookup>> GetCalculationAsync(int categoryId, int subCategoryId)
        {
            var result = await _curtainComponentProductGroupRepository.CalculationByCategory(categoryId, subCategoryId);
            return result;
        }
        public async Task<OrderSummaryDto> GetOrderSummaryByOrderIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderSummaryByOrderIdAsync(orderId);

            if (order == null)
            {
                _logger.LogWarning("هیچ سفارشی با شماره فاکتور {orderId} پیدا نشد.", orderId);
                return null;
            }

            _logger.LogInformation("سفارش با شماره فاکتور {orderId} با موفقیت بارگذاری شد.", orderId);
            return order;
        }
        public async Task<Order> GetOrderByOrderIdAsync(int orderId)
        {
            return await _orderRepository.GetByIdAsync(orderId);
        }
        public async Task UpdateIsFinalyOrderAsync(Order order)
        {
            order.IsFinaly = true;
            order.TotalAmount = order.PriceWithFee + order.DeliveryMethodAmount - order.DisTotal;
            order.TotalCost = order.TotalAmount * order.Count;
            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderSummaryDto>> GetOrderSummaryByUserIdAsync(int userId)
        {
            var order = await _orderRepository.GetOrderSummaryByUserIdAsync(userId);

            if (order == null)
            {
                _logger.LogWarning("هیچ سفارشی با شماره فاکتور {userId} پیدا نشد.", userId);
                return null;
            }

            _logger.LogInformation("سفارش با شماره فاکتور {userId} با موفقیت بارگذاری شد.", userId);
            return order; ;
        }

        public async Task UpdatePriceAndDeliveryAsync(int deliveryId, int orderId)
        {
            try
            {
                await _orderRepository.UpdatePriceAndDeliveryAsync(deliveryId,orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order {OrderId} in OrderService", orderId);
                throw;
            }
        }

        public async Task<int> CountOrders()
        {
            return await _orderRepository.CountOrders();
        }


        public async Task<PagedResult<OrderSummaryDto>> GetOrderSummaryAsync(OrderFilterParameters filter)
        {
            return await _orderRepository.GetOrderSummaryAsync(filter);
        }

        public async Task<PagedResult<OrderSummaryDto>> GetTodayOrdersAsync(int PageNumber = 1, int PageSize = 10)
        {
            return await _orderRepository.GetTodayOrdersAsync(PageNumber, PageSize);
        }

        public class SoftDeleteResult
        {
            public List<int> DeletedOrderIds { get; set; } = new();
            public List<int> DeletedWidthPartIds { get; set; } = new();
            public List<int> DeletedCurtainDetailIds { get; set; } = new();
        }

        public async Task SoftDeleteFromOrderAsync(IEnumerable<int> orderId)
        {
            if (orderId == null || !orderId.Any()) return;
            var allOrders = await _orderRepository.GetAllAsync();
            var ordersToDelete = allOrders.Where(o => orderId.Contains(o.Id) && !o.IsDelete).ToList();
            if (!ordersToDelete.Any()) return;

            var factorIds = ordersToDelete.Select(o => o.Id).Distinct();
            foreach (var item in ordersToDelete)
            {
                MarkAsDeleted(item);
                _orderRepository.Update(item);
            }
            await _orderRepository.SaveChangesAsync();
            var allOrderWidthParts = await _orderWidthPartRepository.GetAllAsync();
            var orderWidthPartsToDelete = allOrderWidthParts.Where(o => orderId.Contains(o.Id) && !o.IsDelete).ToList();
            foreach (var item in orderWidthPartsToDelete)
            {
                MarkAsDeleted(item);
                _orderWidthPartRepository.Update(item);
            }
            await _orderWidthPartRepository.SaveChangesAsync();
            //var allCurtainComponentDetail = await _curtainComponentProductGroupRepository.GetAllAsync(); var curtainComponentDetailToDelete = allCurtainComponentDetail.Where(o => orderId.Contains(o.Id) && !o.IsDelete).ToList(); foreach (var item in curtainComponentDetailToDelete) { item.IsDelete = true; item.LastUpdateDate = DateTime.Now; item.Description = "توسط کاربر حذف شده است."; _curtainComponentProductGroupRepository.Update(item); } await _curtainComponentDetailRepository.SaveChangesAsync();
            return;
        }
        private void MarkAsDeleted(BaseEntity entity)
        {
            entity.IsDelete = true;
            entity.LastUpdateDate = DateTime.Now;
            entity.Description = "توسط کاربر حذف شده است.";
        }


    }

}
