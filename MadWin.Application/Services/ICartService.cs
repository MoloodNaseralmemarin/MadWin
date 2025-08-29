using MadWin.Application.DTOs.Cart;
using Microsoft.AspNetCore.Http;

namespace MadWin.Application.Services
{
    public interface ICartService
    {
        Task<CartResultDto> AddToCart(int productId, ISession session);
        Task<CartResultDto> Increase(int productId, ISession session);
        Task<CartResultDto> Decrease(int productId, ISession session);
        Task<CartResultDto> Remove(int productId, ISession session);
    }
}
