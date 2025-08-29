using System;
using System.Linq;
using MadWin.Infrastructure.Context;
using MadWin.Core.Entities.Properties;
using Microsoft.EntityFrameworkCore;
using MadWin.Core.DTOs.Properties;

namespace Shop2City.Core.Services.Properties
{
    public class PropertyService : IPropertyService
    {
        private readonly MadWinDBContext _context;

        public PropertyService(MadWinDBContext context)
        {
            _context = context;
        }
        public PropertyForAdminViewModel GetProperty(int pageId = 1, string title = "")
        {
            IQueryable<Property> result = _context.Properties.Include(p=>p.PropertyTitle)
                    .Include(p=>p.ProductProperties);
            // Show Item In Page
            var take = 5;
            var skip = (pageId - 1) * take;
            var list = new PropertyForAdminViewModel
            {
                CurrentPage = pageId,
                PageCount = (int) Math.Ceiling(decimal.Divide(result.Count(), take)),
                Properties = result.Skip(skip).Take(take).ToList()
            };
            return list;
        }
    }
}
