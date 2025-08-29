using MadWin.Core.Entities.Properties;

namespace Shop2City.Core.DTOs.PropertyTitles
{
    public class PropertyTitleForAdminViewModel
    {
        public List<PropertyTitle> PropertyTitles { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }
}
