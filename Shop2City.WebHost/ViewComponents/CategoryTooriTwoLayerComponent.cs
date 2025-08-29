using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop2City.Core.Services.Products;

namespace Shop2City.Web.ViewComponents
{
    public class CategoryTooriTwoLayerComponent : ViewComponent
    {
        private readonly IProductService _productService;
        public CategoryTooriTwoLayerComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = _productService.GetAllProductsByGroupId(7);
            if (!products.Any())
                return Content("");
            return await Task.FromResult((IViewComponentResult)View("CategoryTooriTwoLayer", products));
        }
    }
}
