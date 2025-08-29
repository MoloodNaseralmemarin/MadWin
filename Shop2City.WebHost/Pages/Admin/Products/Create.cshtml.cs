using MadWin.Core.Entities.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop2City.Core.Services.Products;

namespace Shop2City.WebHost.Pages.Admin.Products
{
    //[PermissionChecker(11)]
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService;

        public CreateModel(IProductService productService)
        {
            _productService = productService;
        }
        [BindProperty]
        public Product createProduct { get; set; }

        public void OnGet()
        {
            #region Categories

            var productGroup = _productService.GetGroupForManageProduct();
            ViewData["ProductGroup"] = new SelectList(productGroup, "Value", "Text");

            var category = _productService.GetCategoryForManageProduct(int.Parse(productGroup.First().Value));
            ViewData["Categories"] = new SelectList(category, "Value", "Text");

            var subCategory = _productService.GetSubCategoryForManageProduct(int.Parse(category.First().Value));
            ViewData["SubCategory"] = new SelectList(subCategory, "Value", "Text");
            #endregion
        }

        public IActionResult OnPost(bool isStatus)
        {
            if (!ModelState.IsValid)
            {
                var productGroup = _productService.GetGroupForManageProduct();
                ViewData["ProductGroup"] = new SelectList(productGroup, "Value", "Text");
                var category = _productService.GetCategoryForManageProduct(int.Parse(productGroup.First().Value));
                ViewData["Categories"] = new SelectList(category, "Value", "Text");
                var subCategory = _productService.GetSubCategoryForManageProduct(int.Parse(category.First().Value));
                ViewData["SubCategory"] = new SelectList(subCategory, "Value", "Text");
                return Page();
            }
            if (User.Identity == null) return RedirectToPage("Index");
            var userName = User.Identity.Name;
            createProduct.IsStatus = isStatus;
            if (userName != null) _productService.AddProduct(createProduct, userName);
            return RedirectToPage("Index");
        }
    }
}