using MadWin.Core.Entities.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop2City.Core.Services.Products;

namespace Shop2City.Web.Pages.Admin.Products
{
    public class DeleteModel : PageModel
    {
        private readonly IProductService _productService;

        public DeleteModel(IProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public Product DeleteProduct { get; set; }
        public void OnGet(int id)
        {
            DeleteProduct = _productService.GetProductByProductId(id);
        }

        public IActionResult OnPost()
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            _productService.DeleteProduct(DeleteProduct, userId);
            return RedirectToPage("Index");
        }


    }
}