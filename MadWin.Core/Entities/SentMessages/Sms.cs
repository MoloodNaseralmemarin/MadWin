using MadWin.Core.Entities.Common;

namespace MadWin.Core.Entities.SentMessages
{
    public class Sms:BaseEntity
    {
        public string PhoneNumber { get; set; }
        public int SmsCount { get; set; }= 2;
        public string Message { get; set; }
        public string Contact { get; set; }
        public int? OrderId { get; set; }
        public int? FactorId { get; set; }
        public string Status { get; set; } // Sent, Failed, Error, etc.
        public string Template { get; set; }

    }
}
