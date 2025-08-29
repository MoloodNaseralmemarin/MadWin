using MadWin.Core.Entities.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop2City.Core.Services.Products;

namespace Shop2City.WebHost.Pages.Admin.Products
{
    //[PermissionChecker(12)]
    public class EditModel : PageModel
    {
        private IProductService _productService;

        public EditModel(IProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public Product editProduct { get; set; }
      

        public void OnGet(int id)
        {
            editProduct = _productService.GetProductByProductId(id);

            #region Categories
            var groups = _productService.GetGroupForManageProduct();
            ViewData["ProductGroup"] = new SelectList(groups, "Value", "Text",editProduct.ProductGroupId);

            var category = _productService.GetCategoryForManageProduct(int.Parse(groups.First().Value));
            ViewData["Categories"] = new SelectList(category, "Value", "Text", editProduct.Category);

         
                var subCategory = _productService.GetSubCategoryForManageProduct(int.Parse(category.First().Value));
                ViewData["SubCategory"] = new SelectList(subCategory, "Value", "Text", editProduct.SubCategoryId ?? 0);

            #endregion
        }

        public IActionResult OnPost(bool isStatus)
         {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            editProduct.IsStatus = isStatus;
            _productService.UpdateProduct(editProduct, userId);
            return RedirectToPage("Index");
        }
    }
}