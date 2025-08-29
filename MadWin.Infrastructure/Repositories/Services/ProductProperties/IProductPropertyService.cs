using System.Collections.Generic;
using Shop2City.Core.DTOs.ProductPropertise;

namespace Shop2City.Core.Services.ProductProperties
{
    public interface IProductPropertyService
    {
        List<ProductPropertyViewModel> listProductpropertyByProductId(int productId);
    }
}
