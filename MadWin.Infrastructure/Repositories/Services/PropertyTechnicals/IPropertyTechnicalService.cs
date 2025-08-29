using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop2City.Core.DTOs.PropertyTechnicals;

namespace Shop2City.Core.Services.PropertyTechnicals
{
    public interface IPropertyTechnicalService
    {
        PropertyTechnicalForAdminViewModel GetPropertyTechnical(int pageId = 1, string title = "", int topicId = 0);
    }
}
