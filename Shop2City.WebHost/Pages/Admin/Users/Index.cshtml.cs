using MadWin.Application.Services;
using MadWin.Core.DTOs.Users;
using MadWin.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Shop2City.WebHost.Pages.Admin.Users
{
  //  [PermissionChecker(2)]
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;

        public IndexModel(IUserService userService, IUserRepository userRepository)
        {
            _userService = userService;

            _userRepository = userRepository;
        }

        public UserForAdminViewModel userForAdmin { get; set; }

        //public void OnGet(int pageId=1,string filterFirstName = "")
        //{
        //    userForAdmin = _userRepository.GetQuery(pageId, filterFirstName);
        //}

        public IActionResult OnPost(int userId)
        {
         // aw  _userRepository.Remove(userId);
            return RedirectToPage("Index");
        }

    }
}