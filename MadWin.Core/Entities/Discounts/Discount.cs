using MadWin.Core.Entities.Common;
using MadWin.Core.Entities.Users;
using System.ComponentModel.DataAnnotations;

namespace MadWin.Core.Entities.Discounts
{
   public class Discount:BaseEntity
    {
        [Display(Name = "عنوان")]
        public string Item { get; set; } //O سسفارش با انداره دلخواه -D// سفارش آماده

        [Display(Name ="کد تخفیف")]
        public string DiscountCode { get; set; }

        [Display(Name = "درصد کد تخفیف")]
        public int Percentage { get; set; }

        [Display(Name = "تعداد")]
        public int UseableCount { get; set; }

        [Display(Name = "تاریخ شروع")]
        public DateTime StartDate { get; set; }=DateTime.Now;

        [Display(Name = "تاریخ پایان")]
        public DateTime ExpiryDate { get; set; }=DateTime.Now;

        #region Relationship
        public List<UserDiscountCode> UserDiscountCodes { get; set; }
        #endregion



    }
}
