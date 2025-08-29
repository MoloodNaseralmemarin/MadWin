using System.ComponentModel.DataAnnotations;
using MadWin.Core.Entities.Orders;
using MadWin.Core.Entities.Common;
using MadWin.Core.Entities.Products;
using MadWin.Core.Entities.Factors;

namespace MadWin.Core.Entities.Users
{
    public class User:BaseEntity
    {
        #region Ctor
        public User()
        {

        }
        #endregion
        #region Field
        [Display(Name = "نام")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }
        [Display(Name = "شماره همراه")]
        public string CellPhone { get; set; }

        [Display(Name = "شماره ثابت")]
        public string TelPhone { get; set; } = string.Empty;

        [Display(Name = "نام کاربری")]
        public string? UserName { get; set; }

        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }

        [Display(Name = "آدرس پستی")]
        public string Address { get; set; }

        // Computed Property

        #endregion
        #region Relationship

        public virtual List<UserRole> UserRoles { get; set; }
        public virtual List<Product> Products { get; set; }
        public virtual List<Factor> Factors { get; set; }

        public virtual List<Order> OrderModels { get; set; }

        public List<LoginHistory> LoginHistories { get; set; }
        #endregion
    }
}
