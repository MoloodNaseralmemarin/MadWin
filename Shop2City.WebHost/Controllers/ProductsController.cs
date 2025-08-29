using MadWin.Application.Services;
using MadWin.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Shop2City.Core.Services.Products;
using Shop2City.WebHost.ViewModels.Cart;
using System.Security.Claims;


namespace Shop2City.WebHost.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IFactorService _factorService;
        private readonly IUserService _userService;
        private readonly MadWinDBContext _context;


        public ProductsController(IProductService productService, IFactorService factorService, IUserService userService, MadWinDBContext context)
        {
            _productService = productService;
            _factorService = factorService;
            _userService = userService;
            _context = context;
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
            int factorId = 0;
            //var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (!int.TryParse(UserId, out int userId))
            //{
            //    return Unauthorized(); // یا هر رفتار مناسب
            //}

            //var cart = HttpContext.Session.GetJson<List<ShopCartitemViewModel>>("Cart") ?? new List<ShopCartitemViewModel>();
            //foreach (var item in cart)
            //{

            //    factorId = await _factorService.AddFactorAsync(int.Parse(UserId), item.ProductId, item.Count);
            //}
            return RedirectToAction("GetFactorSummary", "Factors", new { area = "UserPanel", factorId = factorId });
        }
    }
}