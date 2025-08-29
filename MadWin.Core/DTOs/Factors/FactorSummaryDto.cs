using MadWin.Core.Entities.Factors;
using System.ComponentModel.DataAnnotations;

namespace MadWin.Core.DTOs.Factors
{
    public class FactorSummaryDto
    {

        public int FactorId { get; set; }
        public string ProductTitle { get; set; }

        /// <summary>
        /// تعداد سفارش داده شده
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        ///قیمت پایه
        /// </summary>
        [Display(Name = "قیمت پایه")]
        public decimal BasePrice { get; set; }

        /// <summary>
        /// قیمت کل محصولات
        /// </summary>
        public decimal FactorSum { get; set; }

        /// <summary>
        /// جمع کل + تعداد
        /// </summary>
        public decimal SubtotalPrice => BasePrice * Count;

        public decimal TotalCost => SubtotalPrice;
        public bool IsFinaly { get; set; }

        public List<FactorDetail> FactorDetails { get; set; }
    }
}
