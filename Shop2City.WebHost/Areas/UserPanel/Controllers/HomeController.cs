using MadWin.Application.DTOs.Orders;
using MadWin.Application.Services;
using MadWin.Core.Interfaces;
using MadWin.Core.Lookups.CommissionRates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop2City.Core.Services.Products;
using Shop2City.Core.Services.UserPanel;


namespace Shop2City.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly ICurtainComponentDetailService _curtainComponentDetailService;
        private readonly ICommissionRateRepository _commissionRateRepository;
  
        private readonly ICurtainComponentRepository _curtainComponentRepository;

        public HomeController(IProductService productService, IOrderService orderService, IOrderRepository orderRepository,
            ICurtainComponentDetailService curtainComponentDetailService,
            ICommissionRateRepository commissionRateRepository,
            ICurtainComponentRepository curtainComponentRepository
           )
        {
            _productService = productService;
            _orderService = orderService;
            _curtainComponentDetailService = curtainComponentDetailService;

            _commissionRateRepository= commissionRateRepository;
            _curtainComponentRepository= curtainComponentRepository;


        }
        public IActionResult Index()
        {
            var category = _productService.GetCategoryForManageProduct(1);
            ViewData["Categories"] = new SelectList(category, "Value", "Text");

            var subCategory = _productService.GetSubCategoryForManageProduct(int.Parse(category.First().Value));
            ViewData["SubCategories"] = new SelectList(subCategory, "Value", "Text");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateOrderInitialDto orderView)
        {
            int orderId = 0;
            int height = 0;
            int width = 0;
            #region بدست اوردن userId
            var UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(UserId, out int userId))
            {
                return Unauthorized(); // یا هر رفتار مناسب
            }

            #endregion
            #region ثبت در جدول سفارش ها و بدست آوردن orderId
            // مرحله 2: ثبت سفارش (بدون مبلغ نهایی، اگر می‌خوای بعداً آپدیت کنی)
            orderId = await _orderService.CreateOrderInitialAsync(orderView, userId, 0); // مقدار اولیه صفر
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


            // مرحله 3: محاسبه هر آیتم
            foreach (var item in items)
            {
               
       
                    Console.WriteLine($"Processing component: {item.CurtainComponentId}");

                    decimal componentCost = item.CurtainComponentId switch
                    {
                        1 => await CalculateIraniAsync(orderView),
                        2 => await CalculateKharejiAsync(orderView),
                        3 => await CalculateTooriOneLayerAsync(orderView),
                        4 => await CalculateTooriTwoLayerAsync(orderView,orderView.PartCount),
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
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine($"ERROR in component {item.CurtainComponentId}: {ex.Message}");
                //}
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
             return RedirectToAction("GetOrderSummary", "Orders", new { area = "UserPanel", orderId = orderId });
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
            decimal a =(decimal)adjustedHeight / 100;
            decimal b =(decimal) a *unitPrice;
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
        private async Task<decimal> CalculateGanCostAsync(int height,int partCount)
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
        private async Task<decimal> CalculateMagnetCostAsync(int height,int partCount)
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

            var adjustedwidth = width+ extraWidth;
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
    }
}