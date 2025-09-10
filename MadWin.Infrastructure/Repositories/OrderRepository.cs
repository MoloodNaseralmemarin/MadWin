using MadWin.Application.Services;
using MadWin.Core.DTOs.Orders;
using MadWin.Core.Entities.Common;
using MadWin.Core.Entities.Orders;
using MadWin.Core.Interfaces;
using MadWin.Core.Lookups.Orders;
using MadWin.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MadWin.Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly MadWinDBContext _context;
        private readonly ILogger<OrderRepository> _logger;
        private readonly INumberRoundingService _numberRoundingService;
        private readonly IDiscountRepository _discountRepository;
        private readonly IDeliveryMethodRepository _deliveryMethodRepository;
        private readonly IOrderWidthPartRepository _orderWidthPartRepository;
        private readonly ICurtainComponentDetailRepository _curtainComponentDetailRepository;

        public OrderRepository(MadWinDBContext context, ILogger<OrderRepository> logger, INumberRoundingService numberRoundingService, IDiscountRepository discountRepository, IDeliveryMethodRepository deliveryMethodRepository, IOrderWidthPartRepository orderWidthPartRepository, ICurtainComponentDetailRepository curtainComponentDetailRepository) : base(context)
        {
            _context = context;
            _logger = logger;
            _numberRoundingService = numberRoundingService;
            _discountRepository = discountRepository;
            _deliveryMethodRepository = deliveryMethodRepository;
            _orderWidthPartRepository = orderWidthPartRepository;
            _curtainComponentDetailRepository = curtainComponentDetailRepository;
        }

        public async Task<int> CreateOrderInitialAsync(Order order)
        {
            await _context.Set<Order>().AddAsync(order);
            await _context.SaveChangesAsync();
            return order.Id;
        }

        public async Task<OrderSummaryDto> GetOrderSummaryByOrderIdAsync(int orderId)
        {
            var order = await _context.Set<Order>()
                .Where(o => o.Id == orderId)
                .Include(o => o.OrderCategory)
                .Include(o => o.OrderSubCategory)
                .Include(o => o.User)
                .Select(o => new OrderSummaryDto
                {
                    FullName = $"{o.User.FirstName + " " + o.User.LastName}",
                    OrderId = o.Id,
                    CategoryGroup = $"{o.OrderCategory.Title} / {o.OrderSubCategory.Title}".Trim(),
                    Size = $"ارتفاع: {o.Height} - عرض: {o.Width}",
                    SizeSMS = $"w: {o.Width} * h: {o.Height}",
                    Count = o.Count,
                    PriceWithFee = o.PriceWithFee,
                    IsEqualParts = o.IsEqualParts,
                    PartCount = o.PartCount,
                    IsFinaly = o.IsFinaly
                }).FirstOrDefaultAsync();

            if (order == null)
                return null;

            var widthParts = await _context.Set<OrderWidthPart>()
                .Where(w => w.OrderId == orderId)
                .Select(w => new OrderWidthPartDto
                {
                    WidthValue = w.WidthValue
                }).ToListAsync();

            order.WidthParts = widthParts;

            return order;
        }

        public async Task UpdatePriceAndCommissionAsync(int orderId, decimal basePrice, int commissionFee, int commissionId)
        {
            var resultBasePrice = _numberRoundingService.RoundLastThreeDigitsToZero(basePrice);

            const int FeePercentage = 100;

            decimal feeAmount = resultBasePrice * commissionFee / FeePercentage;

            var resultfeeAmount = _numberRoundingService.RoundLastThreeDigitsToZero(feeAmount);


            var order = await GetByIdAsync(orderId);
            if (order == null)
                _logger.LogWarning("Update failed: Order with ID {OrderId} not found.", orderId);

            order.BasePrice = resultBasePrice;
            order.PriceWithFee = resultBasePrice + resultfeeAmount;
            order.CommissionRateId = commissionId;
            order.CommissionPercent = commissionFee;
            order.CommissionAmount = resultfeeAmount;

            _context.Set<Order>().Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePriceAndDeliveryAsync(int deliveryId, int orderId)
        {
            var deliveryMethod = await _deliveryMethodRepository.GetDeliveryMethodInfoAsync(deliveryId);

            var order = await GetByIdAsync(orderId);
            if (order == null)
                _logger.LogWarning("Update failed: Order with ID {OrderId} not found.", orderId);

            order.DeliveryMethodId = deliveryMethod.DeliveryId;
            order.DeliveryMethodAmount = deliveryMethod.Cost;
            _context.Set<Order>().Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePriceAndDiscountAsync(int orderId, int discountId)
        {
            var discount = await _discountRepository.GetDiscountInfoByDiscountIdAsync(discountId);

            var order = await GetByIdAsync(orderId);
            if (order == null)
                _logger.LogWarning("Update failed: Order with ID {OrderId} not found.", orderId);

            order.DiscountId = discount.DiscountId;
            order.DisPercent = discount.DiscountPercent;

            #region به دست آوردن قیمت تخفیف
            var price = (order.PriceWithFee * discount.DiscountPercent) / 100;
            #endregion
            order.DisTotal = price;
            _context.Set<Order>().Update(order);
            await _context.SaveChangesAsync();
        }


        public async Task<Order> GetOrderByOrderIdAsync(int orderId)
        {
            return await _context.Set<Order>()
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<OrderInfoLookup> GetOrderInfoByOrderIdAsync(int orderId)
        {
            var order = await _context.Set<Order>()
                .Where(o => o.Id == orderId)
                .Select(o => new OrderInfoLookup
                {
                    OrderId = o.Id,
                    Price = o.PriceWithFee
                }).FirstOrDefaultAsync();

            if (order == null)
                return null;
            return order;
        }

        public async Task<IEnumerable<OrderSummaryDto>> GetOrderSummaryByUserIdAsync(int userId)
        {
            var orders = await _context.Set<Order>()
        .Where(o => o.UserId == userId)
        .Include(o => o.OrderCategory)
        .Include(o => o.OrderSubCategory)
        .Include(o => o.User)
        .Select(o => new OrderSummaryDto
        {
            CreateDate = o.CreateDate,
            OrderId = o.Id,
            FullName = $"{o.User.FirstName} {o.User.LastName}",
            CategoryGroup = $"{o.OrderCategory.Title} / {o.OrderSubCategory.Title}".Trim(),
            Size = $"ارتفاع: {o.Height} - عرض: {o.Width}",
            SizeSMS = $"w: {o.Width} * h: {o.Height}",
            Count = o.Count,
            PriceWithFee = o.PriceWithFee,
            IsEqualParts = o.IsEqualParts,
            PartCount = o.PartCount,
            IsFinaly = o.IsFinaly
        }).ToListAsync();

            if (orders == null || !orders.Any())
                return new List<OrderSummaryDto>();

            var orderIds = orders.Select(o => o.OrderId).ToList();

            var widthParts = await _context.Set<OrderWidthPart>()
                .Where(w => orderIds.Contains(w.OrderId))
                .Select(w => new
                {
                    w.OrderId,
                    w.WidthValue
                }).ToListAsync();

            foreach (var order in orders)
            {
                order.WidthParts = widthParts
                    .Where(wp => wp.OrderId == order.OrderId)
                    .Select(wp => new OrderWidthPartDto
                    {
                        WidthValue = wp.WidthValue
                    }).ToList();
            }

            return orders;
        }
        public async Task<int> CountOrders()
        {
            return await _context.Orders
                .Where(o => o.IsFinaly)
                .CountAsync();
        }

        public async Task<IEnumerable<OrderSummaryDto>> GetOrderSummaryAsync(string fullName)
        {
            var orders = await _context.Set<Order>()
                    .Include(o => o.OrderCategory)
                    .Include(o => o.OrderSubCategory)
                    .Include(o => o.User)
                    .Select(o => new OrderSummaryDto
                    {
                        CreateDate = o.CreateDate,
                        OrderId = o.Id,
                        FullName = $"{o.User.FirstName} {o.User.LastName}",
                        CategoryGroup = $"{o.OrderCategory.Title} / {o.OrderSubCategory.Title}".Trim(),
                        Size = $"ارتفاع: {o.Height} - عرض: {o.Width}",
                        SizeSMS = $"w: {o.Width} * h: {o.Height}",
                        Count = o.Count,
                        PriceWithFee = o.PriceWithFee,
                        IsEqualParts = o.IsEqualParts,
                        PartCount = o.PartCount,
                        IsFinaly = o.IsFinaly
                    }).ToListAsync();

            if (orders == null || !orders.Any())
                return new List<OrderSummaryDto>();

            var orderIds = orders.Select(o => o.OrderId).ToList();

            var widthParts = await _context.Set<OrderWidthPart>()
                .Where(w => orderIds.Contains(w.OrderId))
                .Select(w => new
                {
                    w.OrderId,
                    w.WidthValue
                }).ToListAsync();

            foreach (var order in orders)
            {
                order.WidthParts = widthParts
                    .Where(wp => wp.OrderId == order.OrderId)
                    .Select(wp => new OrderWidthPartDto
                    {
                        WidthValue = wp.WidthValue
                    }).ToList();
            }

            return orders;
        }

        public async Task<PagedResult<OrderSummaryDto>> GetOrderSummaryAsync(OrderFilterParameters filter)
        {
            var query = _context.Orders
                .Include(o => o.OrderCategory)
                .Include(o => o.OrderSubCategory)
                .Include(o => o.User)
                .AsQueryable();

            // فیلترها
            if (!string.IsNullOrWhiteSpace(filter.FullName))
            {
                query = query.Where(o =>
                    (o.User.FirstName + " " + o.User.LastName).Contains(filter.FullName));
            }

            if (filter.OrderId.HasValue)
            {
                query = query.Where(o => o.Id == filter.OrderId.Value);
            }

            if (filter.FromDate.HasValue)
            {
                query = query.Where(o => o.CreateDate >= filter.FromDate.Value);
            }

            if (filter.ToDate.HasValue)
            {
                query = query.Where(o => o.CreateDate <= filter.ToDate.Value);
            }

            if (filter.FromPrice.HasValue)
            {
                query = query.Where(o => o.PriceWithFee >= filter.FromPrice.Value);
            }

            if (filter.ToPrice.HasValue)
            {
                query = query.Where(o => o.PriceWithFee <= filter.ToPrice.Value);
            }

            int totalCount = await query.CountAsync();

            var skip = (filter.PageNumber - 1) * filter.PageSize;

            var orders = await query
                .OrderByDescending(o => o.CreateDate)
                .Skip(skip)
                .Take(filter.PageSize)
                .Select(o => new OrderSummaryDto
                {
                    CreateDate = o.CreateDate,
                    OrderId = o.Id,
                    FullName = $"{o.User.FirstName} {o.User.LastName}",
                    CategoryGroup = $"{o.OrderCategory.Title} / {o.OrderSubCategory.Title}".Trim(),
                    Size = $"ارتفاع: {o.Height} - عرض: {o.Width}",
                    SizeSMS = $"w: {o.Width} * h: {o.Height}",
                    Count = o.Count,
                    PriceWithFee = o.PriceWithFee,
                    IsEqualParts = o.IsEqualParts,
                    PartCount = o.PartCount,
                    IsFinaly = o.IsFinaly
                }).ToListAsync();

            var orderIds = orders.Select(o => o.OrderId).ToList();

            var widthParts = await _context.Set<OrderWidthPart>()
                .Where(w => orderIds.Contains(w.OrderId))
                .Select(w => new
                {
                    w.OrderId,
                    w.WidthValue
                }).ToListAsync();

            foreach (var order in orders)
            {
                order.WidthParts = widthParts
                    .Where(wp => wp.OrderId == order.OrderId)
                    .Select(wp => new OrderWidthPartDto
                    {
                        WidthValue = wp.WidthValue
                    }).ToList();
            }

            return new PagedResult<OrderSummaryDto>
            {
                Items = orders,
                TotalCount = totalCount
            };
        }

        public async Task<PagedResult<OrderSummaryDto>> GetTodayOrdersAsync(int PageNumber = 1, int PageSize = 10)
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var query = _context.Orders
                .Include(o => o.OrderCategory)
                .Include(o => o.OrderSubCategory)
                .Include(o => o.User)
                .Where(o => o.CreateDate >= today && o.CreateDate < tomorrow)
                .AsQueryable();

            int totalCount = await query.CountAsync();

            var skip = (PageNumber - 1) * PageSize;

            var orders = await query
                .OrderByDescending(o => o.CreateDate)
                .Skip(skip)
                .Take(PageSize)
                .Select(o => new OrderSummaryDto
                {
                    CreateDate = o.CreateDate,
                    OrderId = o.Id,
                    FullName = $"{o.User.FirstName} {o.User.LastName}",
                    CategoryGroup = $"{o.OrderCategory.Title} / {o.OrderSubCategory.Title}".Trim(),
                    Size = $"ارتفاع: {o.Height} - عرض: {o.Width}",
                    SizeSMS = $"w: {o.Width} * h: {o.Height}",
                    Count = o.Count,
                    PriceWithFee = o.PriceWithFee,
                    IsEqualParts = o.IsEqualParts,
                    PartCount = o.PartCount,
                    IsFinaly = o.IsFinaly
                }).ToListAsync();

            var orderIds = orders.Select(o => o.OrderId).ToList();

            var widthParts = await _context.Set<OrderWidthPart>()
                .Where(w => orderIds.Contains(w.OrderId))
                .Select(w => new
                {
                    w.OrderId,
                    w.WidthValue
                }).ToListAsync();

            foreach (var order in orders)
            {
                order.WidthParts = widthParts
                    .Where(wp => wp.OrderId == order.OrderId)
                    .Select(wp => new OrderWidthPartDto
                    {
                        WidthValue = wp.WidthValue
                    }).ToList();
            }

            return new PagedResult<OrderSummaryDto>
            {
                Items = orders,
                TotalCount = totalCount
            };
        }

        public async Task SoftDeleteFromOrderAsync(IEnumerable<int> orderIds)
        {
            //if (orderIds == null || !orderIds.Any())
            //    return;

            //// سفارش‌ها
            //var orders = await _orderRepository.GetQuery()
            //    .Where(o => orderIds.Contains(o.Id) && !o.IsDelete)
            //    .ToListAsync();

            //foreach (var o in orders)
            //    MarkAsDeleted(o);

            //_orderRepository.UpdateRange(orders);

            //// عرض‌ها
            //var widthParts = await _orderWidthPartRepository.GetQuery()
            //    .Where(w => orderIds.Contains(w.OrderId) && !w.IsDelete)
            //    .ToListAsync();

            //foreach (var w in widthParts)
            //    MarkAsDeleted(w);

            //_orderWidthPartRepository.UpdateRange(widthParts);

            //// جزئیات پرده
            //var curtainDetails = await _curtainComponentDetailRepository.GetQuery()
            //    .Where(c => orderIds.Contains(c.OrderId) && !c.IsDelete)
            //    .ToListAsync();

            //foreach (var c in curtainDetails)
            //    MarkAsDeleted(c);

            //_curtainComponentDetailRepository.UpdateRange(curtainDetails);

            //// ذخیره
            //await _orderRepository.SaveChangesAsync();
        }

        private void MarkAsDeleted(BaseEntity entity)
        {
            entity.IsDelete = true;
            entity.LastUpdateDate = DateTime.Now;
            entity.Description = "توسط کاربر حذف شده است.";
        }
    }
}