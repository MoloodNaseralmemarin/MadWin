using Microsoft.AspNetCore.Mvc;
using Shop2City.WebHost.ViewModels.Cart;

namespace Shop2City.WebHost.ViewComponents
{
    public class CartBadgeComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()

        {
            // خواندن سبد خرید از سشن
            var cart = HttpContext.Session.GetObjectFromJson<List<ShopCartitemViewModel>>("Cart")
                    ?? new List<ShopCartitemViewModel>();

            var model = new CartBadgeViewModel
            {
                TotalCount = cart.Sum(c => c.Count),
                TotalPrice = cart.Sum(c => c.Sum)
            };
            return await Task.FromResult((IViewComponentResult)View("CartBadge", model));
        }
    }
}
