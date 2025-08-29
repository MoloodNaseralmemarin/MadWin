using MadWin.Core.Entities.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop2City.Core.Services.Products;

namespace Shop2City.WebHost.Pages.Admin.ProductGroups
{
    public class CreateModel : PageModel
    {
        public readonly  IProductService _ProductService;
        public CreateModel(IProductService productService)
        {
            _ProductService = productService;
        }

        [BindProperty]
        public ProductGroup createProductGroup { get; set; }


        public void OnGet(int? id)
        {
            createProductGroup = new ProductGroup()
            {
                ParentId = id
            };
        }

        public IActionResult OnPost()
        {
            _ProductService.AddProductGroups(createProductGroup);
            return RedirectToPage("Index");
        }
    }
}