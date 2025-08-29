using MadWin.Application.Services;
using MadWin.Core.Common;
using MadWin.Core.DTOs.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop2City.Core.Services.Permissions;

namespace Shop2City.WebHost.Pages.Admin.Users
{
    //[PermissionChecker(3)]
    public class CreateModel : PageModel
    {
        private  IUserService _userService;
        private  IPermissionService _permissionService;

        public CreateModel(IUserService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }

        [BindProperty]
        public CreateUserViewModel createUser { get; set; }

        public void OnGet()
        {
            ViewData["Roles"] = _permissionService.GetRoles();
        }

        public async Task<IActionResult> OnPost(List<int> SelectedRoles)
        {
            if (!ModelState.IsValid)
                return Page();
            if (await _userService.IsExistCellPhoneAsync(createUser.cellPhone))
            {
                ModelState.AddModelError("PhoneNumber", ErrorMessage.InvalidCellPhone);
                ViewData["Roles"] = _permissionService.GetRoles();
                return Page();
            }



            //if (createUser.userName != null && _userService.IsExistUserName(createUser.userName))
            //{
            //    ModelState.AddModelError("userName", ErrorMessage.InvalidUserName);
            //    ViewData["Roles"] = _permissionService.GetRoles();
            //    return Page();
            //}
            int userId = 1;// await _userService.AddUserFromAdmin(createUser);
            //TODO: Add Roles
            #region Add Roles
            _permissionService.AddRolesToUser(SelectedRoles, userId);
            #endregion
            return Redirect("/Admin/Users");

        }
    }
}