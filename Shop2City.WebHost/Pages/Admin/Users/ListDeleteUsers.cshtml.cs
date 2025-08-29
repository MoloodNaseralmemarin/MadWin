using MadWin.Application.Services;
using MadWin.Core.DTOs.Users;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Shop2City.Web.Pages.Admin.Users
{
    public class ListDeleteUsersModel : PageModel
    {
        private IUserService _userService;

        public ListDeleteUsersModel(IUserService userService)
        {
            _userService = userService;
        }

        public UserForAdminViewModel userForAdmin { get; set; }

        public async void OnGet(int pageId = 1, string filterUserName = "", string filterEmail = "")
        {
           // userForAdmin =await _userService.GetDeleteUsers(pageId,filterEmail,filterUserName);
        }

    }
}