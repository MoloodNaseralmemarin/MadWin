
using MadWin.Core.Entities.Common;
using MadWin.Core.Entities.Orders;

namespace MadWin.Core.Entities.CurtainComponents
{
    public class CurtainComponentDetail : BaseEntity
    {
        public int OrderId { get; set; }

        public int CurtainComponentId { get; set; }
        public decimal UnitCost { get; set; }

        public int Count { get; set; }
        public decimal FinalCost { get; set; }


        public Order Order { get; set; }

        public CurtainComponent CurtainComponent { get; set; }
    }
}
