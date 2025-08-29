using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MadWin.Core.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop2City.Core.Security;
using Shop2City.Core.Services.Permissions;

namespace Shop2City.Web.Pages.Admin.Roles
{
    [PermissionChecker(8)]
    public class EditModel : PageModel
    {
        private IPermissionService _permissionService;

        public EditModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [BindProperty]
        public Role editRole { get; set; }
        public void OnGet(int id)
        {
            editRole = _permissionService.GetRoleById(id);
            ViewData["Permissions"] = _permissionService.GetPermissions();
            ViewData["SelectedPermissions"] = _permissionService.PermissionsRole(id);
        }

        public IActionResult OnPost(List<int> SelectedPermission)
        {
            if (!ModelState.IsValid)
                return Page();


            _permissionService.UpdateRole(editRole);

            _permissionService.UpdatePermissionsToRole(editRole.roleId,SelectedPermission);

            return RedirectToPage("Index");
        }
    }
}