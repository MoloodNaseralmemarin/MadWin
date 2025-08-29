using MadWin.Application.DTOs.Cart;
using MadWin.Application.Services;
using MadWin.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Shop2City.Core.Services.Products;
using Shop2City.WebHost.ViewModels.Cart;

namespace Shop2City.WebHost.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var result = await _cartService.AddToCart(productId, HttpContext.Session);
            return Json(result);
        }

        public IActionResult ShowCart()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<ShopCartitemDto>>("Cart")
                       ?? new List<ShopCartitemDto>();

            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> Increase(int productId)
        {
            var result = await _cartService.Increase(productId, HttpContext.Session);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Decrease(int productId)
        {
            var result = await _cartService.Decrease(productId, HttpContext.Session);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int productId)
        {
            var result = await _cartService.Remove(productId, HttpContext.Session);
            return Json(result);
        }





    }
}