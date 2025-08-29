using MadWin.Infrastructure.Context;
using MadWin.Core.Entities.Properties;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop2City.Core.Services.PropertyTechnicalProducts
{
    public class PropertyTechnicalProductService : IPropertyTechnicalProductService
    {
        private readonly MadWinDBContext _context;
        public PropertyTechnicalProductService(MadWinDBContext context)
        {
            _context = context;
        }
        public List<PropertyTechnicalProduct> listPropertyTechnicalProductByProductId(int productId)
        {
            var q = _context.PropertyTechnicalProducts.Where(a => a.productId == productId)
               // .Include(a => a.PropertyTechnical)
                .ToList();
                return q;
        }
    }
}
