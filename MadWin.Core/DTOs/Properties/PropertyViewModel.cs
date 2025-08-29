

using MadWin.Core.Entities.Properties;

namespace MadWin.Core.DTOs.Properties
{
    public class PropertyForAdminViewModel
    { 
        public List<Property> Properties { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }
}
