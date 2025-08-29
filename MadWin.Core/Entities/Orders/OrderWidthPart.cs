using MadWin.Core.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;


namespace MadWin.Core.Entities.Orders
{
    [Table("OrderWidthParts", Schema = "Orders")]
    public class OrderWidthPart:BaseEntity
    {
        public int WidthValue { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

    }
}
