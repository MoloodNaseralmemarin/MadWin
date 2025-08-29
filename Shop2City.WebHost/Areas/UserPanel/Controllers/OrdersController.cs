using MadWin.Application.Services;
using MadWin.Core.DTOs.Orders;
using MadWin.Core.Entities.SentMessages;
using MadWin.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop2City.WebHost.ViewModels.Orders;
using System.Security.Claims;

namespace Shop2City.WebHost.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IDeliveryMethodService _deliveryMethodService;
        private readonly ILogger<OrdersController> _logger;
        private readonly IDiscountService _disCountService;

        public OrdersController(IOrderService orderService, IDeliveryMethodService deliveryMethodService,ILogger<OrdersController> logger, IDiscountService disCountService)
        {
            _orderService = orderService;
            _deliveryMethodService = deliveryMethodService;
            _logger = logger;
            _disCountService = disCountService;
        }

        public async Task<IActionResult> GetOrderSummary(int orderId)
        {
            var orderSummary = await _orderService.GetOrderSummaryByOrderIdAsync(orderId);  // از پارامتر استفاده شد
            var deliveryMethods = await _deliveryMethodService.GetDeliveryMethodInfoAsync();
            if (orderSummary == null)
                return NotFound();

            var viewModel = new OrderSummaryViewModel
            {
                OrderSummary = orderSummary,
                DeliveryMethods = deliveryMethods
            };

            return View(viewModel);
        }
        #region کد تخفیف
        public async Task<IActionResult> UseDiscountAsync(int orderId, string discountCode)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
                return Json(new { success = false, message = "کاربر نامعتبر" });

            var result = await _disCountService.UseDiscountAsync(orderId, discountCode, userId);
            

            switch (result)
            {
                case DiscountUseType.Success:

                    var applyDiscount = await _disCountService.ApplyDiscountAsync(orderId, discountCode);
                    
                    return Json(new
                    {
                        success = true,
                        discountAmount = applyDiscount.DiscountAmount,
                        message = "کد تخفیف با موفقیت اعمال شد"
                    });

                case DiscountUseType.ExpirationDate:
                    return Json(new
                    {
                        success = false,
                        message = "مهلت استفاده از کد تخفیف به پایان رسیده است"
                    });

                case DiscountUseType.NotFound:
                    return Json(new
                    {
                        success = false,
                        message = "کد تخفیف یافت نشد"
                    });

                case DiscountUseType.Finished:
                    return Json(new
                    {
                        success = false,
                        message = "سقف استفاده از این کد تخفیف به پایان رسیده است"
                    });

                case DiscountUseType.UserUsed:
                    return Json(new
                    {
                        success = false,
                        message = "شما قبلاً از این کد تخفیف استفاده کرده‌اید"
                    });

                default:
                    return Json(new
                    {
                        success = false,
                        message = "خطای ناشناخته‌ای رخ داده است"
                    });
            }
        }
        #endregion

        [Authorize]
        public async Task<IActionResult> ShowOrderForUser()
        {
            #region بدست اوردن userId
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(UserId, out int userId))
            {
                return Unauthorized(); // یا هر رفتار مناسب
            }
            #endregion
            var order =await _orderService.GetOrderSummaryByUserIdAsync(userId);
            return View(order);
        }


    }
}

