using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MadWin.Core.Entities.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop2City.Core.Services.Products;

namespace Shop2City.WebHost.Pages.Admin.ProductGroups
{
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;
        public EditModel(IProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public ProductGroup editProductGroup { get; set; }


        public void OnGet(int id)
        {
            editProductGroup = _productService.GetProductGroupById(id);
        }

        public IActionResult OnPost()
        { 
            _productService.UpdateProductGroups(editProductGroup);
            return RedirectToPage("Index");

        }
    }
}