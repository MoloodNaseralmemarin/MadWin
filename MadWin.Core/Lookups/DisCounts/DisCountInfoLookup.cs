using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Core.Lookups.Discounts
{
    public class DiscountInfoLookup
    {
        public int DiscountId { get; set; }

        [Display(Name = "کد تخفیف")]
        public string DiscountCode { get; set; }

        [Display(Name = "درصد کد تخفیف")]
        public int DiscountPercent { get; set; }

        [Display(Name = "تعداد")]
        public int UseableCount { get; set; }

        [Display(Name = "تاریخ شروع")]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Display(Name = "تاریخ پایان")]
        public DateTime EndDate { get; set; } = DateTime.Now;
    }
}
