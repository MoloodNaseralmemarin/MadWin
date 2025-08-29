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
    [PermissionChecker(9)]
    public class DeleteModel : PageModel
    {
        private IPermissionService _permissionService;

        public DeleteModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [BindProperty]
        public Role deleteRole { get; set; }
        public void OnGet(int id)
        {
            deleteRole = _permissionService.GetRoleById(id);
        }

        public IActionResult OnPost()
        {
            _permissionService.DeleteRole(deleteRole);

            return RedirectToPage("Index");
        }
    }
}