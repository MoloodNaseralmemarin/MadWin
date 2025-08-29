

using MadWin.Core.Entities.Products;

namespace Shop2City.Core.Services.ProductImages
{
    public interface IProductImageService
    {

        List<ProductGallery> ListProductImageByProductId(int productId);
        void AddProductImage(ProductGallery productImage);
    }
}
