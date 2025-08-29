using MadWin.Core.Entities.Common;

namespace MadWin.Core.Entities.Advices
{
    public class AdviceModel:BaseEntity
    {
        public string Status { get; set; }
        public string ReturnId { get; set; }
        public string Message { get; set; }
    }
}
