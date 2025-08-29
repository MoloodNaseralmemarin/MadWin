using MadWin.Core.Entities.Products;
using MadWin.Core.Entities.Users;
using MadWin.Core.Interfaces;
using MadWin.Core.Lookups.Account;
using MadWin.Core.Lookups.Products;
using MadWin.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly MadWinDBContext _context;
        public ProductRepository(MadWinDBContext context):base(context)
        {
            _context=context;
        }
        public async Task<ProductInfoLookup> GetProductInfoByProductId(int productId)
        {
            var item = await _context.Set<Product>()
         .AsNoTracking()
         .FirstOrDefaultAsync(u => u.Id == productId);

            if (item == null)
                return null;

            return new ProductInfoLookup
            {
                ProductId=item.Id,
                Title=item.Title,
                Price=item.Price
            };
        }
    }
}
