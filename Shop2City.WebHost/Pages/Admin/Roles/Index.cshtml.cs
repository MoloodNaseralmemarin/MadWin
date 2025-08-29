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
    //[PermissionChecker(6)]
    public class IndexModel : PageModel
    {
        private IPermissionService _permissionService;

        public IndexModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public List<Role> listRoles { get; set; }

       
        public void OnGet()
        {
            listRoles = _permissionService.GetRoles();
        }

       
    }
}