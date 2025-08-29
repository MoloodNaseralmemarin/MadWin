using MadWin.Core.Common;
using System.ComponentModel.DataAnnotations;

namespace Shop2City.WebHost.ViewModels.Orders
{
    public class OrderViewModel
    {
        [Display(Name = "گروه اصلی")]
        [Required(ErrorMessage = "انتخاب گروه اصلی الزامی می باشد")]
        public int CategoryId { get; set; }

        [Display(Name = "گروه فرعی")]
        [Required(ErrorMessage = "انتخاب گروه فرعی الزامی می باشد")]
        public int SubCategoryId { get; set; }

        [Display(Name = "ارتفاع")]
        [Required(ErrorMessage = "وارد کردن اندازه ارتفاع الزامی می باشد")]
        [Range(0, 400, ErrorMessage = ErrorMessage.Range)]
        public int Height { get; set; }

        [Display(Name = "عرض")]
        [Required(ErrorMessage = "وارد کردن اندازه عرض الزامی می باشد")]
        [Range(0, 400, ErrorMessage = ErrorMessage.Range)]

        public int Width { get; set; }

        [Display(Name = "تعداد")]

        [Range(1, 400, ErrorMessage = ErrorMessage.Range)]
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
        /// مبلغ کارمزد
        /// </summary>
        public decimal CommissionAmount { get; set; }

        public List<float> WidthParts { get; set; }
    }
}