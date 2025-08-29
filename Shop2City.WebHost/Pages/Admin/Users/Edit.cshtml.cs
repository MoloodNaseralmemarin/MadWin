using MadWin.Application.Services;
using MadWin.Core.DTOs.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop2City.Core.Services.Permissions;

namespace Shop2City.Web.Pages.Admin.Users
{
   // [PermissionChecker(4)]
    public class EditModel : PageModel
    {
        private IUserService _userService;
        private IPermissionService _permissionService;

        public EditModel(IUserService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }
        [BindProperty]
        public EditUserViewModel editUser { get; set; }
        public void OnGet(int id)
        {
           // editUser = _userService.GetUserForShowEditMode(id);
            ViewData["Roles"] = _permissionService.GetRoles();
        }

        public IActionResult OnPost(List<int> SelectedRoles)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Roles"] = _permissionService.GetRoles();
                return Page();
            }
            //if ( _userService.IsExistCellPhoneAsync(editUser.cellPhone))
            //{
            //    ModelState.AddModelError("PhoneNumber", ErrorMessage.InvalidCellPhone);
            //    ViewData["Roles"] = _permissionService.GetRoles();
            //    return Page();
            //}



            //if (_userService.IsExistUserName(editUser.userName))
            //{
            //    ModelState.AddModelError("userName", ErrorMessage.InvalidUserName);
            //    ViewData["Roles"] = _permissionService.GetRoles();
            //    return Page();
            //}
           // _userService.EditUserFromAdmin(editUser);

            //Edit Roles
            _permissionService.EditRolesToUser(editUser.userId,SelectedRoles);
            return RedirectToPage("Index");
        }
    }
}