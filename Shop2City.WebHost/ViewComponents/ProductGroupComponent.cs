using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop2City.Core.Services.Products;

namespace Shop2City.Web.ViewComponents
{
    public class ProductGroupComponent:ViewComponent
    {
        private readonly IProductService _productService;

        public ProductGroupComponent(IProductService productService)
        {
            _productService = productService;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult) View("ProductGroup", _productService.GetAllGroup()));
        }
    }
}
