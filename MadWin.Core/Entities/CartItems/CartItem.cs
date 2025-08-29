using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Core.Entities.CartItems
{
    namespace Core.Entities
    {
        public class CartItem
        {
            public int ProductId { get; private set; }
            public string Title { get; private set; }
            public string ImageName { get; private set; }
            public int Count { get; private set; }
            public decimal PricePerUnit { get; private set; }
            public decimal Price => Count * PricePerUnit;

            public CartItem(int productId, string title, string imageName, decimal pricePerUnit, int count = 1)
            {
                ProductId = productId;
                Title = title;
                ImageName = imageName;
                PricePerUnit = pricePerUnit;
                Count = count;
            }

            public void Increase(int amount = 1)
            {
                Count += amount;
            }

            public void Decrease(int amount = 1)
            {
                Count -= amount;
                if (Count < 0) Count = 0;
            }
        }
    }

}
