using MadWin.Core.Entities.Common;
using MadWin.Core.Entities.Discounts;
using MadWin.Core.Entities.Factors;
using MadWin.Core.Entities.Orders;

namespace MadWin.Core.Entities.Users
{
   public class UserDiscountCode : BaseEntity
    {
        public UserDiscountCode()
        {

        }
        public int UserId { get; set; }

        public int? OrderId {  get; set; }
        public int? FactorId { get; set; }
        public int DisCountId { get; set; }

        #region Relationship
        public User User { get; set; }

        public Factor Factor { get; set; }
        public Order Order { get; set; }
        public Discount Discount { get; set; }
        #endregion

    }
}
