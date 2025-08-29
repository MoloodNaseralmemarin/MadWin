using MadWin.Core.Entities.CartItems.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Core.Interfaces
{
    public interface ICartRepository
    {
        List<CartItem> GetCart(ISession session);
        void SaveCart(ISession session, List<CartItem> cart);
    }
}
