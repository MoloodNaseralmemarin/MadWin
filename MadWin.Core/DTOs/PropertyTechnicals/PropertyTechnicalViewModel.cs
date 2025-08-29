using MadWin.Core.Entities.Properties;

namespace Shop2City.Core.DTOs.PropertyTechnicals
{
    public class PropertyTechnicalForAdminViewModel
    { 
        public List<PropertyTechnical> PropertyTechnicals { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }
}
