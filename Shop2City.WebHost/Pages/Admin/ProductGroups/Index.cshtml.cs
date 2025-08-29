using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MadWin.Core.Entities.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop2City.Core.Services.Products;

namespace Shop2City.Web.Pages.Admin.ProductGroups
{
    public class IndexModel : PageModel
    {

        private IProductService _productService;

        public IndexModel(IProductService productService)
        {
            _productService= productService;
        }

        public List<ProductGroup> ProductGroups { get; set; }

        public void OnGet()
        {
            ProductGroups = _productService.GetAllGroup();
        }

        
    }
}