using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Core.Lookups.CommissionRates
{
    public class CommissionInfoLookup
    {
        public int CommissionRateId { get; set; }
        public int CommissionPercent { get; set; }
        public decimal CommissionAmount { get; set; }
    }
}
