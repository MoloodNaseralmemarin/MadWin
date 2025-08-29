namespace MadWin.Core.DTOs.Orders
{
    public class OrderSummaryDto
    {
        public DateTime CreateDate { get; set; }
        public string FullName { get; set; }
        public int OrderId { get; set; }

        /// <summary>
        /// گروه‌بندی کامل شامل گروه و زیر گروه (مثلاً "پرده / پرده اتاق خواب")
        /// </summary>
        public string CategoryGroup { get; set; }

        /// <summary>
        /// ارتفاع * عرض
        /// </summary>
        public string Size { get; set; }

        public string SizeSMS { get; set; }

        /// <summary>
        /// تعداد سفارش داده شده
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// تعداد تکه‌ها (قطعات جداگانه)
        /// </summary>
        public int PartCount { get; set; }

        /// <summary>
        /// آیا تعداد تکه‌ها برابر است
        /// </summary>
        public bool IsEqualParts { get; set; }

        /// <summary>
        /// قیمت پایه به‌علاوه قیمت کارمزد (جمع کل قیمت واحد)
        /// </summary>
        public decimal PriceWithFee { get; set; }

        /// <summary>
        /// جمع کل + تعداد
        /// </summary>
        public decimal SubtotalPrice => PriceWithFee * Count;

        public List<OrderWidthPartDto> WidthParts { get; set; } = new();
        /// <summary>
        /// وضعیت نهایی سفارش (آیا سفارش قطعی است)
        /// </summary>
        public bool IsFinaly { get; set; }

        public decimal TotalCost { get; set; }
    }

    public class OrderWidthPartDto
    {
        public int WidthValue { get; set; }
    }
}
