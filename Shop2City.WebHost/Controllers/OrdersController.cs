using MadWin.Application.DTOs.Orders;
using MadWin.Application.Repositories;
using MadWin.Application.Services;
using MadWin.Core.DTOs.Orders;
using MadWin.Core.Interfaces;
using MadWin.Core.Lookups.CommissionRates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop2City.Core.Services.Products;
using Shop2City.Core.Services.UserPanel;
using Shop2City.WebHost.ViewModels.Orders;
using System.Security.Claims;


namespace Shop2City.Web.Areas.UserPanel.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IUserPanelService _userPanelService;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly ICurtainComponentDetailService _curtainComponentDetailService;
        private readonly IDeliveryMethodService _deliveryMethodService;
        private readonly ILogger<OrdersController> _logger;
        private readonly IDiscountService _disCountService;
        private readonly ICommissionRateRepository _commissionRateRepository;

        private readonly ICurtainComponentRepository _curtainComponentRepository;

        public OrdersController(IUserService userService, IUserPanelService userPanelService, IProductService productService, IOrderService orderService, IOrderRepository orderRepository,
            ICurtainComponentDetailService curtainComponentDetailService,
            IDeliveryMethodService deliveryMethodService,
            ILogger<OrdersController> logger,
            IDiscountService disCountService,
            ICommissionRateRepository commissionRateRepository
           )
        {
            _userService = userService;
            _userPanelService = userPanelService;
            _productService = productService;
            _curtainComponentDetailService = curtainComponentDetailService;
            _orderService = orderService;
            _deliveryMethodService = deliveryMethodService;
            _logger = logger;
            _disCountService = disCountService;
            _commissionRateRepository = commissionRateRepository;


        }
        public IActionResult CreateOrder()
        {
            var category = _productService.GetCategoryForManageProduct(1);
            ViewData["Categories"] = new SelectList(category, "Value", "Text");

            var subCategory = _productService.GetSubCategoryForManageProduct(int.Parse(category.First().Value));
            ViewData["SubCategories"] = new SelectList(subCategory, "Value", "Text");
            return View();
        }

        #region محاسبات

        #endregion
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderInitialDto orderView)
        {
            int orderId = 0;
            #region بدست اوردن userId
            var UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(UserId, out int userId))
            {
                return Unauthorized(); // یا هر رفتار مناسب
            }

            #endregion
            #region اگر ارتفاع کمتر از 200 بود 200 محاسبه شود و عرض کمتر از 80 بود 80 محاسبه شود

            if (orderView.Height < 200)
            {
                orderView.Height = 200;

            }
            if (orderView.Width < 80)
            {
                orderView.Width = 80;
            }
            #endregion
            decimal basePrice = 0;
            var items = await _orderService.GetCalculationAsync(orderView.CategoryId, orderView.SubCategoryId);

            if (items == null || !items.Any())
                return View();

            #region ثبت در جدول سفارش ها و بدست آوردن orderId
            // مرحله 2: ثبت سفارش (بدون مبلغ نهایی، اگر می‌خوای بعداً آپدیت کنی)
            orderId = await _orderService.CreateOrderInitialAsync(orderView, userId, 0); // مقدار اولیه صفر
            #endregion
            // مرحله 3: محاسبه هر آیتم
            foreach (var item in items)
            {
                try
                {
                    Console.WriteLine($"Processing component: {item.CurtainComponentId}");

                    decimal componentCost = item.CurtainComponentId switch
                    {
                        1 => await CalculateIraniAsync(orderView),
                        2 => await CalculateKharejiAsync(orderView),
                        3 => await CalculateTooriOneLayerAsync(orderView),
                        4 => await CalculateTooriTwoLayerAsync(orderView, orderView.PartCount),
                        5 => await CalculateZipper5CostAsync(orderView.Width),
                        6 => await CalculateZipper2CostAsync(orderView.Width),
                        7 => await CalculateChodonCostAsync(orderView.Width),
                        8 => await CalculateGanCostAsync(orderView.Height, orderView.PartCount),
                        9 => await CalculateMagnetCostAsync(orderView.Height, orderView.PartCount),
                        10 => await CalculateGlue4CostAsync(orderView.Width),
                        11 => await CalculateGlue2CostAsync(orderView.Height),
                        12 => await GetWageCostAsync(),
                        13 => await GetPackagingCostAsync(),
                        _ => 0
                    };

                    basePrice += componentCost;

                    Console.WriteLine($"Saving component: {item.CurtainComponentId} with cost {componentCost}");

                    await _curtainComponentDetailService.CreateCurtainComponentDetailInitialAsync(orderId, item.CurtainComponentId, componentCost, orderView.Count);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR in component {item.CurtainComponentId}: {ex.Message}");
                }
            }

            #region به دست آوردن مبلغ کارمزد نسب به تعداد تکه و مساوی/نامساوی
            var commission = await GetCommissionInfoAsync(orderView.PartCount, orderView.IsEqualParts);
            #endregion
            #region ویرایش قیمت پایه + قیمت کارمزد + شناسه کارمزد
            await _orderService.UpdatePriceAndCommissionAsync(orderId, basePrice, commission.CommissionPercent, commission.CommissionRateId);
            #endregion
            var category = _productService.GetCategoryForManageProduct(1);
            ViewData["Categories"] = new SelectList(category, "Value", "Text");

            var subCategory = _productService.GetSubCategoryForManageProduct(int.Parse(category.First().Value));
            ViewData["SubCategories"] = new SelectList(subCategory, "Value", "Text");
            var model = await _orderService.GetTodayOrdersAsync(1, 10);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return ViewComponent("OrderSummary");

            return View();
        }
        #region پرده طلقی ایرانی
        private async Task<decimal> CalculateIraniAsync(CreateOrderInitialDto order)
        {
            var unitPrice = await _curtainComponentRepository.GetPriceByIdAsync(1);
            if (unitPrice <= 0)
                return 0;
            return ((order.Height + 10) * order.Width * unitPrice) / 10000;
        }
        #endregion
        #region پرده طلقی خارجی
        private async Task<decimal> CalculateKharejiAsync(CreateOrderInitialDto order)
        {
            var unitPrice = await _curtainComponentRepository.GetPriceByIdAsync(2);
            if (unitPrice <= 0)
                return 0;
            return ((order.Height + 10) * order.Width * unitPrice) / 10000;
        }
        #endregion
        #region پرده توری یک لایه
        private async Task<decimal> CalculateTooriOneLayerAsync(CreateOrderInitialDto order)
        {
            var unitPrice = await _curtainComponentRepository.GetPriceByIdAsync(3);
            if (unitPrice <= 0)
                return 0;
            return ((order.Height + 10) * order.Width * unitPrice) / 10000;
        }
        #endregion
        #region پرده توری دو لایه
        private async Task<decimal> CalculateTooriTwoLayerAsync(CreateOrderInitialDto order, int partCount)
        {
            decimal totalCost = 0;

            // محاسبه هزینه لایه دوم (با ضخامت 40 و قیمت ID = 4)
            decimal unitPrice = await _curtainComponentRepository.GetPriceByIdAsync(4);
            if (unitPrice <= 0)
                return 0;
            decimal heightPlusMargin = order.Height + 10;
            decimal twoLayerArea = heightPlusMargin * 40;
            decimal twoLayerCost = (twoLayerArea / 10000) * unitPrice;

            // نصف قیمت گان
            //decimal halfGanCost = unitPrice / 2;  به گفته آقای نادری در تارخ 03/26

            //totalCost += twoLayerCost + halfGanCost;

            // هزینه پرده توری یک لایه (ID = 3)
            decimal singleLayerCostPerMeter = await _curtainComponentRepository.GetPriceByIdAsync(3);
            decimal singleLayerCost = (heightPlusMargin * order.Width * singleLayerCostPerMeter) / 10000;
            totalCost += singleLayerCost + twoLayerCost;

            #region برای پرده دولایه 3قسمت مساوی/نامساوی

            #endregion

            //// سایر اجزا
            totalCost += await CalculateZipper5CostAsync(order.Width);
            totalCost += await CalculateZipper2CostAsync(order.Height);
            totalCost += await CalculateChodonCostAsync(order.Width);
            totalCost += await CalculateGanCostAsync(order.Height, order.PartCount);
            totalCost += await CalculateMagnetCostAsync(order.Height, order.PartCount);
            totalCost += await CalculateGlue4CostAsync(order.Width);
            totalCost += await CalculateGlue2CostAsync(order.Height);
            totalCost += await GetWageCostAsync();
            totalCost += await GetPackagingCostAsync();

            var isTriplePart = partCount == 3;
            decimal a = totalCost + twoLayerCost;
            return isTriplePart ? a : totalCost;
        }
        #endregion
        #region زیپ چسب 5 سانت
        private async Task<decimal> CalculateZipper5CostAsync(int width)
        {
            const decimal coefficient = 0.01M;
            const int extraWidth = 5;

            var unitPrice = await _curtainComponentRepository.GetPriceByIdAsync(5);
            if (unitPrice <= 0)
                return 0;

            return (((width + extraWidth) * coefficient) * unitPrice);
        }
        #endregion
        #region زیپ چسب 2.5 سانت
        private async Task<decimal> CalculateZipper2CostAsync(int height)
        {
            var unitPrice = await _curtainComponentRepository.GetPriceByIdAsync(6);
            if (unitPrice <= 0)
                return 0;
            var adjustedHeight = GetAdjustedHeight(height);
            //decimal a =adjustedHeight / 100;
            //decimal b = a *unitPrice;
            decimal a = (decimal)adjustedHeight / 100;
            decimal b = (decimal)a * unitPrice;
            return b;
        }
        #endregion
        #region جودون
        private async Task<decimal> CalculateChodonCostAsync(int width)
        {
            const decimal coefficient = 0.01M;
            const int extraWidth = 2;

            var unitPrice = await _curtainComponentRepository.GetPriceByIdAsync(7);
            if (unitPrice <= 0)
                return 0;
            var adjustedWidth = width + extraWidth;

            decimal resultChodon = adjustedWidth * coefficient * unitPrice;

            return resultChodon;
        }
        #endregion
        #region گان
        private async Task<decimal> CalculateGanCostAsync(int height, int partCount)
        {
            const decimal coefficient = 0.01M;
            const int extraHeight = 10;
            const decimal widthFactor = 4.2M;  //وزن هر متر گان     
            const int quantity = 4;

            var unitPrice = await _curtainComponentRepository.GetPriceByIdAsync(8);

            if (unitPrice <= 0)
                return 0;

            int adjustedHeight = height + extraHeight;
            decimal resultGan = adjustedHeight * coefficient * widthFactor * quantity * unitPrice;

            var newGan = resultGan / 2;


            var isTriplePart = partCount == 3;
            decimal a = resultGan + newGan;
            return isTriplePart ? a : resultGan;
        }

        #endregion
        #region آهنربا
        private async Task<decimal> CalculateMagnetCostAsync(int height, int partCount)
        {
            const decimal magnetSpacing = 13.5M;
            const int threshold1 = 200;
            const int threshold2 = 400;

            var unitPrice = await _curtainComponentRepository.GetPriceByIdAsync(9);
            if (unitPrice <= 0)
                return 0;

            decimal result = 0;

            if (height >= 0 && height <= threshold1)
            {
                int effectiveHeight = height - 20;
                if (effectiveHeight > 0)
                {
                    int magnetCount = (int)Math.Round(effectiveHeight / magnetSpacing) * 2;
                    result = magnetCount * unitPrice;
                }
            }
            else if (height > threshold1 && height <= threshold2)
            {
                int effectiveHeight = height - 30;
                if (effectiveHeight > 0)
                {
                    int magnetCount = (int)Math.Ceiling(effectiveHeight / magnetSpacing) * 2;
                    result = magnetCount * unitPrice;
                }
            }
            else
            {
                Console.WriteLine("خطا: ارتفاع خارج از محدوده مجاز است.");
            }
            var isTriplePart = partCount == 3;
            return isTriplePart ? result * 2 : result;
        }
        #endregion
        #region چسب 2 طرفه 4 سانت
        private async Task<decimal> CalculateGlue4CostAsync(int width)
        {
            const decimal coefficient = 0.01M;
            const int extraWidth = 5;

            var unitPrice = await _curtainComponentRepository.GetPriceByIdAsync(10);

            var adjustedwidth = width + extraWidth;
            decimal resultGlue4 = adjustedwidth * coefficient * unitPrice;

            return resultGlue4;
        }
        #endregion
        #region چسب 2 طرفه 2 سانت
        private async Task<decimal> CalculateGlue2CostAsync(int height)
        {
            const decimal coefficient = 100;

            var adjustedHeight = GetAdjustedHeight(height);

            var unitPrice = await _curtainComponentRepository.GetPriceByIdAsync(11);
            decimal resultGlue2 = adjustedHeight / coefficient * unitPrice;

            return resultGlue2;
        }
        #endregion
        #region اجرت دوخت
        public async Task<decimal> GetWageCostAsync()
        {
            var cost = await _curtainComponentRepository.GetPriceByIdAsync(12);
            return cost;
        }
        #endregion
        #region   #region اجرت بسته بندی
        public async Task<decimal> GetPackagingCostAsync()
        {
            var cost = await _curtainComponentRepository.GetPriceByIdAsync(13);


            return cost;
        }
        #endregion

        #region محاسبه ارتفاع برای زیپ چسب 2.5 سانت و گان
        public int GetAdjustedHeight(int height)
        {
            int heightNew = 0;
            switch (height)
            {
                case int n when n >= 0 && n <= 230:
                    heightNew = 90;
                    break;
                case int n when n >= 231 && n <= 270:
                    heightNew = 110;
                    break;
                case int n when n >= 271 && n <= 300:
                    heightNew = 125;
                    break;
                case int n when n >= 301 && n <= 330:
                    heightNew = 145;
                    break;
                case int n when n >= 331 && n <= 360:
                    heightNew = 165;
                    break;
                case int n when n >= 361 && n <= 400:
                    heightNew = 185;
                    break;
                default:
                    Console.WriteLine("error");
                    break;
            }
            return heightNew;
        }
        #endregion
        #region محاسبه کارمزد
        private async Task<CommissionInfoLookup> GetCommissionInfoAsync(int partCount, bool isEqualParts)
        {
            return await _commissionRateRepository.GetCommissionInfoAsync(partCount, isEqualParts);
        }
        #endregion
  
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
            var order = await _orderService.GetOrderSummaryByUserIdAsync(userId);
            return View(order);
        }
        #region حذف از سفارش
        public async Task<IActionResult> RemoveItemsByOrderAsync(int[] orderIds)
        {
            return null;
            //await _orderService.SoftDeleteFromOrderAsync(orderIds);
        }
        #endregion
    }
}