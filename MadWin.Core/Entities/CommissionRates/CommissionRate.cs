

using MadWin.Core.Entities.Common;
using MadWin.Core.Entities.Orders;

namespace MadWin.Core.Entities.CommissionRates
{
    public class CommissionRate :BaseEntity
    {
        public int PartCount { get; set; }              // تعداد تکه (2 یا 3)
        public bool IsEqualParts { get; set; }          // آیا تکه‌ها مساوی‌اند؟
        public int CommissionPercent { get; set; }   // درصد کارمزد

        public string ShortDescription { get; set; }

        public List<Order> Orders { get; set; }
    }
}
