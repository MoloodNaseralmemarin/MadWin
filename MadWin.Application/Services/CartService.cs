using MadWin.Application.DTOs.Cart;
using MadWin.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MadWin.Application.Services
{
    public class CartService : ICartService
    {
        private readonly IProductRepository _productRepository;
        public CartService(IProductRepository productRepository)
        {

            _productRepository = productRepository;

        }
        public async Task<CartResultDto> AddToCart(int productId, ISession session)
        {
            var product = await _productRepository.GetProductInfoByProductId(productId);
            if (product == null)
                throw new Exception("Product not found");

            var cart = session.GetObjectFromJson<List<ShopCartitemDto>>("Cart") ?? new List<ShopCartitemDto>();

            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Count += 1;
            }
            else
            {
                cart.Add(new ShopCartitemDto
                {
                    ProductId = productId,
                    Title = product.Title,
                    Count = 1,
                    Price = product.Price
                });
            }

            session.SetJson("Cart", cart);

            return new CartResultDto
            {
                TotalCount = cart.Sum(c => c.Count),
                TotalPrice = cart.Sum(c => c.Price * c.Count)
            };
        }



        public Task<CartResultDto> Increase(int productId, ISession session)
        {
            var cart = session.GetObjectFromJson<List<ShopCartitemDto>>("Cart") ?? new List<ShopCartitemDto>();
            var item = cart.FirstOrDefault(c => c.ProductId == productId);

            if (item != null)
            {
                item.Count++;
                session.SetJson("Cart", cart);
            }

            return Task.FromResult(new CartResultDto
            {
                TotalCount = cart.Sum(c => c.Count),
                TotalPrice = cart.Sum(c => c.Price * c.Count)
            });
        }

        public Task<CartResultDto> Decrease(int productId, ISession session)
        {
            var cart = session.GetObjectFromJson<List<ShopCartitemDto>>("Cart") ?? new List<ShopCartitemDto>();
            var item = cart.FirstOrDefault(c => c.ProductId == productId);

            if (item != null)
            {
                if (item.Count > 1)
                    item.Count--;
                else
                    cart.Remove(item);

                session.SetJson("Cart", cart);
            }

            return Task.FromResult(new CartResultDto
            {
                TotalCount = cart.Sum(c => c.Count),
                TotalPrice = cart.Sum(c => c.Price * c.Count)
            });
        }

        public Task<CartResultDto> Remove(int productId, ISession session)
        {
            var cart = session.GetObjectFromJson<List<ShopCartitemDto>>("Cart") ?? new List<ShopCartitemDto>();
            var item = cart.FirstOrDefault(c => c.ProductId == productId);

            if (item != null)
            {
                cart.Remove(item);
                session.SetJson("Cart", cart);
            }

            return Task.FromResult(new CartResultDto
            {
                TotalCount = cart.Sum(c => c.Count),
                TotalPrice = cart.Sum(c => c.Price * c.Count)
            });
        }

    }
}
