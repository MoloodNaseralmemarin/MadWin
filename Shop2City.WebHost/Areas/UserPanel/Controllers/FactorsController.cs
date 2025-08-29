using MadWin.Application.Services;
using MadWin.Core.DTOs.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop2City.WebHost.ViewModels.Factors;
using System.Security.Claims;

namespace Shop2City.WebHost.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class FactorsController : Controller
    {
        private readonly IFactorDetailService _factorDetailService;
        private readonly IDeliveryMethodService _deliveryMethodService;
        private readonly IDiscountService _discountService;
        public FactorsController(IFactorDetailService factorDetailService, IDeliveryMethodService deliveryMethodService, IDiscountService discountService)
        {
            _factorDetailService = factorDetailService;
            _deliveryMethodService = deliveryMethodService;
            _discountService = discountService;
        }

        public async Task<IActionResult> GetFactorSummary(int factorId)

        {
            var factorSummary = await _factorDetailService.GetFactorSummaryByFactorIdAsync(factorId);  // از پارامتر استفاده شد
            var deliveryMethods = await _deliveryMethodService.GetDeliveryMethodInfoAsync();
            if (factorSummary == null)
                return NotFound();

            var viewModel = new FactorSummaryViewModel
            {
                FactorSummary = factorSummary,
                DeliveryMethods = deliveryMethods
            };

            return View(viewModel);
        }

        #region کد تخفیف
        public async Task<IActionResult> UseDiscountForFactorAsync(int factorId, string discountCode)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
                return Json(new { success = false, message = "کاربر نامعتبر" });

            var result = await _discountService.UseDiscountForFactorAsync(factorId, discountCode, userId);


            switch (result)
            {
                case DiscountUseType.Success:

                    var applyDiscount = await _discountService.ApplyDiscountForFactorAsync(factorId, discountCode);

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


    }
}

