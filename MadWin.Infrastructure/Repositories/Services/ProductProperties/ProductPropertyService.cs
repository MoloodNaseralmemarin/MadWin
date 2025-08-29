using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop2City.Core.DTOs.ProductPropertise;
using MadWin.Infrastructure.Context;

namespace Shop2City.Core.Services.ProductProperties
{
    public class ProductPropertyService : IProductPropertyService
    {
        private readonly MadWinDBContext _context;
        public ProductPropertyService(MadWinDBContext context)
        {
            _context = context;
        }
        public List<ProductPropertyViewModel> listProductpropertyByProductId(int productId)
        {
            //var q =_context.ProductProperties
            //    .Include(prop)
            //    (from a in db.TblPropertiseTitle
            //         join b in db.TblPropertis on a.ID equals b.TitleID
            //         join c in db.TblPropertis_Product on b.ID equals c.PropertiseID
            //         where c.ProductID == ProductID
            //         select new VmPropertice
            //         {
            //             ID = a.ID,
            //             IDProduct_Property = c.ID,
            //             Title = a.Title,
            //             PropTitle = b.Title,
            //             Price = c.Price
            //         }).ToList();

            return null;
        }
    }
}
