using System;
using System.Linq;
using MadWin.Infrastructure.Context;
using MadWin.Core.Entities.Properties;
using Microsoft.EntityFrameworkCore;
using Shop2City.Core.DTOs.PropertyTechnicals;

namespace Shop2City.Core.Services.PropertyTechnicals
{
    public class PropertyTechnicalService : IPropertyTechnicalService
    {
        private readonly MadWinDBContext _context;

        public PropertyTechnicalService(MadWinDBContext context)
        {
            _context = context;
        }
        public PropertyTechnicalForAdminViewModel GetPropertyTechnical(int pageId = 1, string title = "", int topicId = 0)
        {
           
            IQueryable<PropertyTechnical> result = _context.propertyTechnicals
                .Include(p=>p.PropertyTechnicalProducts);
            // Show Item In Page
            var take = 5;
            var skip = (pageId - 1) * take;
            var list = new PropertyTechnicalForAdminViewModel
            {
                CurrentPage = pageId,
                PageCount = (int)Math.Ceiling(decimal.Divide(result.Count(), take)),
                PropertyTechnicals = result.Skip(skip).Take(take).ToList()
            };
            return list;
        }
    }
}
