using MadWin.Application.Services;
using MadWin.Core.DTOs.Orders;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop2City.Core.Security;
using System.Threading.Tasks;

namespace Shop2City.WebHost.Pages.Admin
{
    [PermissionChecker(1)]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }

}
