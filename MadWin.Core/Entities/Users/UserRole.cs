using System.ComponentModel.DataAnnotations;

namespace MadWin.Core.Entities.Users
{
   public class UserRole
    {
        #region Ctor
        public UserRole()
        {
        }
        #endregion

        #region Field
        [Key]
        public int userRoleId { get; set; }
        public int userId { get; set; }
        public int roleId { get; set; }
        #endregion

        #region Relationship
        public virtual User user { get; set; }
        public virtual Role role { get; set; }
        #endregion

    }
}
