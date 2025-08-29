
using MadWin.Core.Entities.Common;
using MadWin.Core.Entities.Products;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MadWin.Core.Entities.Users;
using MadWin.Core.Entities.DeliveryMethods;
using MadWin.Core.Entities.CommissionRates;
using MadWin.Core.Entities.Discounts;

namespace MadWin.Core.Entities.Orders
{

    [Table("Orders", Schema = "Orders")]
    public class Order : BaseEntity
    {
        public int UserId { get; set; }

        [Display(Name = "گروه اصلی")]
        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Count { get; set; }
        /// <summary>
        /// تعداد تکه
        /// </summary>
        public int PartCount { get; set; }
        /// <summary>
        /// آیا مساوی اند
        /// </summary>
        public bool IsEqualParts { get; set; }

        /// <summary>
        ///قیمت پایه
        /// </summary>
        [Display(Name = "قیمت پایه")]
        public decimal BasePrice { get; set; }
        /// <summary>
        /// شناسه قیمت کارمزد
        /// </summary>
        public int CommissionRateId { get; set; }
        /// <summary>
        /// درصد کارمزد
        /// </summary>
        public int CommissionPercent { get; set; }
        /// <summary>
        /// مبلغ کارمزد
        /// </summary>
        public decimal CommissionAmount { get; set; }

        /// <summary>
        /// قیمت پایه + محاسبه کارمزد
        /// </summary>
        public decimal PriceWithFee { get; set; }
        /// <summary>
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
        /// قیمت نهایی
        /// </summary>
        public decimal TotalAmount { get; set; } = 0;
        /// <summary>
        /// قیمت نهایی * تعداد
        /// </summary>
        public decimal TotalCost { get; set; } = 0;

        /// <summary>
        /// وضعیت پرداخت
        /// </summary>
        public bool IsFinaly { get; set; }
        public User User { get; set; }

        #region Order
        [ForeignKey(nameof(CategoryId))]
        public ProductGroup OrderCategory { get; set; }

        [ForeignKey(nameof(SubCategoryId))]
        public ProductGroup OrderSubCategory { get; set; }
        #endregion


        public DeliveryMethod DeliveryMethod { get; set; }

        public CommissionRate CommissionRate { get; set; }

        public Discount Discount { get; set; }

        public ICollection<OrderWidthPart> WidthParts { get; set; } = new List<OrderWidthPart>();
    }
}
