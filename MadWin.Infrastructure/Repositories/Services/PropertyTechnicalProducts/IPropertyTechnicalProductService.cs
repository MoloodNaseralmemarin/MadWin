using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MadWin.Core.Entities.Properties;


namespace Shop2City.Core.Services.PropertyTechnicalProducts
{
    public interface IPropertyTechnicalProductService
    {
        List<PropertyTechnicalProduct> listPropertyTechnicalProductByProductId(int productId);
    }
}
