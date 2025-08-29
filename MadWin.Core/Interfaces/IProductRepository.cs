using MadWin.Core.Entities.Products;
using MadWin.Core.Entities.Users;
using MadWin.Core.Lookups.Account;
using MadWin.Core.Lookups.Products;

namespace MadWin.Core.Interfaces
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        Task<ProductInfoLookup> GetProductInfoByProductId(int productId);

    }
}
