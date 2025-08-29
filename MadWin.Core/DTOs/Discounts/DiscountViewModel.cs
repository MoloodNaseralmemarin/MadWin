using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Core.DTOs.DisCounts
{
    public class DisCountViewModel
    {
        public int id { get; set; }
        public DateTime createDate { get; set; }
        public string item { get; set; }

        public int disCountPercent { get; set; }

        public string disCountCode { get; set; }

        public int useableCount { get; set; }
        public DateTime startDate {get;set;}
        public DateTime endDate { get; set; }
        public string isInactive { get; set; }

    }

    public class DiscountResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public decimal NewTotal { get; set; }
    }
}
