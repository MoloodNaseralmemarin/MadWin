using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MadWin.Core.Entities.Permissions
{
    public class Permission
    {
        [Key]
        public int permissionId { get; set; }

        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string PermissionTitle { get; set; }
        public int? parentId { get; set; }


        [ForeignKey("parentId")]
        public List<Permission> permissions { get; set; }

        public List<RolePermission> rolePermissions { get; set; }


    }
}
