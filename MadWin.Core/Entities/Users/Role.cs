using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MadWin.Core.Entities.Users
{ 
    public class Role
    {

        #region Ctor
        public Role()
        {

        }

        #endregion

        #region Field
        [Key]
        public int roleId { get; set; }
        [Display(Name = "عنوان نقش")]
        public string roleTitle { get; set; }

        public bool isDelete { get; set; }

        #endregion

        #region Relationship
        public List<UserRole> userRoles { get; set; }
        #endregion
    }
}
