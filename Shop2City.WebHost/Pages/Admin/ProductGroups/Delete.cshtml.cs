using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MadWin.Core.Entities.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop2City.Core.Services.Products;

namespace Shop2City.Web.Pages.Admin.ProductGroups
{
    public class DeleteModel : PageModel
    {
        private readonly IProductService _productService;

        public DeleteModel(IProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public ProductGroup deleteProductGroup { get; set; }
        public void OnGet(int id)
        {
            deleteProductGroup = _productService.GetProductGroupById(id);
        }

        public IActionResult OnPost()
        {
            _productService.DeleteProductGroups(deleteProductGroup);
            return RedirectToPage("Index");
        }


    }
}