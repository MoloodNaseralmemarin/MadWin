using MadWin.Core.Entities.Common;
using MadWin.Core.Entities.DeliveryMethods;
using MadWin.Core.Entities.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MadWin.Core.Entities.Factors
{

    [Table("Factors", Schema = "Factors")]
    public class Factor:BaseEntity
    {
        #region ctor
        public Factor()
        {

        }
        #endregion
        #region Field
        [Required]
        public int UserId { get; set; }

        /// شناسه نوع پست
        /// </summary>
        public int DeliveryMethodId { get; set; }
        /// <summary>
        /// قیمت نوع پست
        /// </summary>
        public decimal DeliveryMethodAmount { get; set; }

        /// <summary>
        /// شناسه کد تخفیف
        /// </summary>
        public int DiscountId { get; set; }

        /// <summary>
        /// درصد کد تخفیف
        /// </summary>
        public int DisPercent { get; set; }

        /// <summary>
        /// قیمت تخفیف محاسبه شده
        /// </summary>
        public decimal DisTotal { get; set; }

        /// <summary>
        /// قیمت کل محصولات
        /// </summary>
        [Required]
        public decimal FactorSum { get; set; }
        /// <summary>
        /// فاکتور پرداخت کرده و فاکتور نهایی شده
        /// </summary>
        public bool IsFinaly { get; set; }
        /// <summary>
        /// قیمت نهایی  جمع کل قیمت ها
        /// </summary>
        public decimal TotalAmount { get; set; } = 0;
        #endregion
        #region Relationship
        public virtual User User { get; set; }
        public List<FactorDetail> FactorDetails { get; set; }
        public virtual List<UserDiscountCode> UserDiscountCodes { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        #endregion

    }
}
