using MadWin.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop2City.Core.Services.Products;

namespace Shop2City.WebHost.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IDeliveryMethodService _deliveryMethodService;
        public HomeController(IProductService productService, IDeliveryMethodService deliveryMethodService)
        {
            _productService = productService;
            _deliveryMethodService = deliveryMethodService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("ShowProduct/{id}")]
        public IActionResult ShowProduct(int id)
        {
            var product = _productService.GetProductForShow(id);
            var productGalleries = _productService.GetProductActiveGalleries(id);


            return View(Tuple.Create(product, productGalleries));
        }

        [HttpGet]
        public IActionResult ContactUs()
        {
            return View();
        }

        #region GetCategory

        //گروه اصلی
        public IActionResult GetCategory(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem() {Text = "گروه اصلی را انتخاب کنید", Value = ""}
            };
            list.AddRange(_productService.GetCategoryForManageProduct(id));
            return Json(new SelectList(list, "Value", "Text"));
        }

        #endregion

        #region GetSubCategory

        public IActionResult GetSubCategory(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem() {Text = "گروه فرعی را انتخاب کنید", Value = ""}
            };
            list.AddRange(_productService.GetSubCategoryForManageProduct(id));

            return Json(new SelectList(list, "Value", "Text"));
        }

        #endregion




        [HttpGet]
        public async Task<IActionResult> GetDeliveryPriceAsync(int deliveryId)
        {
            var pricePost = await _deliveryMethodService.GetDeliveryMethodCostByIdAsync(deliveryId);

            return Ok(new
            {
                success = true,
                price = pricePost
            });
        }

        [Route("Error")]
        public IActionResult Error()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Error.html"), "text/html");
        }

    }
}
