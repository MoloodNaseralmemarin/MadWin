using MadWin.Application.DTOs.Cart;
using MadWin.Application.Services;
using MadWin.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop2City.Core.Services.Products;
using System.Security.Claims;


namespace Shop2City.WebHost.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IFactorService _factorService;
        private readonly IUserService _userService;
        private readonly IServiceProvider _serviceProvider;

        public ProductsController(IProductService productService, IFactorService factorService, IUserService userService, IServiceProvider serviceProvider)
        {
            _productService = productService;
            _factorService = factorService;
            _userService = userService;
            _serviceProvider = serviceProvider;
        }
        public IActionResult Index(int pageId = 1, string filterProductTitleFa = ""
           , List<int> selectedGroups = null)
        {
            ViewBag.selectedGroups = selectedGroups;
            ViewBag.FilterProductTitleFa = filterProductTitleFa;
            ViewBag.Groups = _productService.GetAllGroup();
            ViewBag.list = _productService.ShowMainProductGroups();
            ViewBag.pageId = pageId;
            ViewData["Referer"] = Request.Headers["Path"].ToString();
            return View(_productService.GetProduct(pageId, filterProductTitleFa, selectedGroups));
        }
        [Authorize]
        public async Task<IActionResult> BuyProduct()
        {
            // دریافت UserId از claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(); // اگر کاربر لاگین نکرده باشد
            }

            // دریافت سبد خرید از session
            var cart = HttpContext.Session.GetObjectFromJson<List<ShopCartitemDto>>("Cart") ?? new List<ShopCartitemDto>();

            // اگر سبد خرید خالی باشد
            if (!cart.Any())
            {
                return RedirectToAction("Index", "Home"); // کاربر را به صفحه اصلی هدایت کن
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var factorService = scope.ServiceProvider.GetRequiredService<IFactorService>();

                // فقط یکبار فاکتور رو بساز یا بیار
                var factorId = await factorService.CreateOrGetOpenFactorAsync(userId);

                // حالا همه‌ی آیتم‌ها رو اضافه کن
                foreach (var item in cart)
                {
                    await factorService.AddOrUpdateFactorDetailAsync(factorId, item.ProductId, item.Count);
                }

                // آپدیت مجموع فاکتور
                await factorService.UpdateFactorSumAsync(factorId);

                // هدایت به صفحه خلاصه فاکتور
                return RedirectToAction("GetFactorSummary", "Factors", new { factorId });
            }
        }
    }
}