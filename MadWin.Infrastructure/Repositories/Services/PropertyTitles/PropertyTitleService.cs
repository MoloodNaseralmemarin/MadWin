using System;
using System.Linq;
using MadWin.Infrastructure.Context;
using MadWin.Core.Entities.Properties;
using Microsoft.EntityFrameworkCore;
using Shop2City.Core.DTOs.PropertyTitles;

namespace Shop2City.Core.Services.PropertyTitles
{
    public class PropertyTitleService : IPropertyTitleService
    {
        private readonly MadWinDBContext _context;

        public PropertyTitleService(MadWinDBContext context)
        {
            _context = context;
        }
       public PropertyTitleForAdminViewModel GetPropertyTitle(int pageId = 1, string title = "")
        {
            IQueryable<PropertyTitle> result = _context.PropertyTitles.Include(pt=>pt.Properties);
            // Show Item In Page
            var take = 5;
            var skip = (pageId - 1) * take;
            var list = new PropertyTitleForAdminViewModel
            {
                CurrentPage = pageId,
                PageCount = (int)Math.Ceiling(decimal.Divide(result.Count(), take)),
                PropertyTitles = result.Skip(skip).Take(take).ToList()
            };
            return list;
        }
    }
}
