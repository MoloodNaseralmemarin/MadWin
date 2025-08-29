using MadWin.Core.DTOs.Products;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop2City.Core.DTOs.Products;
using Shop2City.Core.Services.Products;

namespace Shop2City.WebHost.Pages.Admin.Products
{
    // [PermissionChecker(10)]
    public class IndexModel : PageModel
    {
        private readonly  IProductService _productService;


        public IndexModel(IProductService productService)
        {
            _productService = productService;

        }
        public ProductForAdminViewModel ProductForAdminViewModel { get; set; }

        public void OnGet(int pageId = 1, string filterSerialNumber = "", string filterProductTitleFa = "")
        {
            ProductForAdminViewModel = _productService.GetProductForAdmin(pageId, filterSerialNumber, filterProductTitleFa);
        }
    }
}