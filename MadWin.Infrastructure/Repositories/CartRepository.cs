using MadWin.Core.Entities.CartItems.Core.Entities;
using MadWin.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        public List<CartItem> GetCart(ISession session)
        {
            return session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
        }

        public void SaveCart(ISession session, List<CartItem> cart)
        {
            session.SetJson("Cart", cart);
        }

    }
}
