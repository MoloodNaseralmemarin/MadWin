using MadWin.Infrastructure.Context;
using MadWin.Core.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace Shop2City.Core.Services.ProductImages
{
    public class ProductImageService : IProductImageService
    {
        private readonly MadWinDBContext _context;

        public ProductImageService(MadWinDBContext context)
        {
            _context = context;
        }

        public List<ProductGallery> ListProductImageByProductId(int productId)
        {
            var GetlistImage = _context.ProductGalleries
                .Where(a => a.ProductId == productId)
                .ToList();
            return GetlistImage;
        }

        public void AddProductImage(ProductGallery productImage)
        {
            _context.ProductGalleries.Add(productImage);
            _context.SaveChanges();
        }
    }
}
