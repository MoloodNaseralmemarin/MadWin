using MadWin.Core.Entities.Factors;
using System.ComponentModel.DataAnnotations;

namespace MadWin.Core.DTOs.Factors
{
    public class FactorSummaryDto
    {
        public int FactorId { get; set; }

        // لیست جزئیات (هر محصول)
        public List<FactorDetailDto> FactorDetails { get; set; } = new();

        // جمع کل قبل از تخفیف
        public decimal SubTotal => FactorDetails.Sum(d => d.TotalPrice);

        // مبلغ تخفیف (فعلاً صفر، بعداً با کد تخفیف پر میشه)
        public decimal Discount { get; set; } = 0;

        // هزینه ارسال (بعداً براساس روش ارسال محاسبه میشه)
        public decimal DeliveryPrice { get; set; } = 0;

        // مبلغ قابل پرداخت = جمع کل - تخفیف + ارسال
        public decimal FinalTotal => SubTotal - Discount + DeliveryPrice;
    }


    public class FactorDetailDto
    {
        public int Id { get; set; }
        public string ProductTitle { get; set; } = "";
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public decimal TotalPrice => Price * Quantity;
    }

}
