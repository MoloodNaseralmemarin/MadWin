using MadWin.Application.DTOs.Orders;
using MadWin.Core.DTOs.Calculations;
using MadWin.Core.DTOs.Orders;
using MadWin.Core.Entities.Discounts;
using MadWin.Core.Entities.Orders;
using MadWin.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace MadWin.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly ICurtainComponentProductGroupRepository _curtainComponentProductGroupRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderWidthPartRepository _orderWidthPartRepository;
        private readonly ILogger<OrderService> _logger;


        public OrderService(ICurtainComponentProductGroupRepository curtainComponentProductGroupRepository,IOrderRepository orderRepository, IOrderWidthPartRepository orderWidthPartRepository, ILogger<OrderService> logger)
        {
             _curtainComponentProductGroupRepository = curtainComponentProductGroupRepository;
            _orderRepository = orderRepository;
            _orderWidthPartRepository = orderWidthPartRepository;
            _logger = logger;
        }

        public async Task<int> CreateOrderInitialAsync(CreateOrderInitialDto dto,int userId,decimal basePrice)
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
            await _orderRepository.SaveChangesAsync();
            foreach (var width in dto.WidthParts)
            {
                order.WidthParts.Add(new OrderWidthPart
                {
                    WidthValue = width
                });
            }
           
            await _orderWidthPartRepository.SaveChangesAsync();
           
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
    }
}