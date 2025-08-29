using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop2City.Core.DTOs.PropertyTitles;

namespace Shop2City.Core.Services.PropertyTitles
{
    public interface IPropertyTitleService
    {
        PropertyTitleForAdminViewModel GetPropertyTitle(int pageId = 1, string title = "");
    }
}
