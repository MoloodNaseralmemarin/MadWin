using MadWin.Core.Entities.Users;
using System.ComponentModel.DataAnnotations;

namespace MadWin.Core.Entities.Permissions
{
   public class RolePermission
    {
        [Key]
        public int rolePermissionId { get; set; }
        public int roleId { get; set; }
        public int permissionId { get; set; }

        public Role role { get; set; }
        public Permission permission { get; set; }

    }
}
