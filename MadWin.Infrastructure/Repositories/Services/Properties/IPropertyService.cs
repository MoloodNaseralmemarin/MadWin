using MadWin.Core.DTOs.Properties;

namespace Shop2City.Core.Services.Properties
{
   public interface IPropertyService
    {
        PropertyForAdminViewModel GetProperty(int pageId = 1, string title = "");
    }
}
