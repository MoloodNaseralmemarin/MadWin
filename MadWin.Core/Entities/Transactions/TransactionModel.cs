using MadWin.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Core.Entities.Transactions
{
    public class TransactionModel :BaseEntity
    {
        public string TerminalId { get; set; }
        public int InvoiceId { get; set; }
        public string Amount { get; set; }
        public string CardNumber { get; set; }
        public string Payload { get; set; }
        public string Rrn { get; set; }
        public string TraceNumber { get; set; }
        public string DigitalReceipt { get; set; }
        public string DatePaid { get; set; }
        public string RespCode { get; set; }
        public string RespMsg { get; set; }
        public string IssuerBank { get; set; }
    }
}
