using MadWin.Core.Common;
using MadWin.Core.Entities.Common;
using MadWin.Core.Entities.Factors;
using MadWin.Core.Entities.Orders;
using System.ComponentModel.DataAnnotations;

namespace MadWin.Core.Entities.DeliveryMethods
{
    public class DeliveryMethod:BaseEntity
    {
        public DeliveryMethod()
        {
                
        }
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(200, ErrorMessage = ErrorMessage.MaxLength)]
        public string Name { get; set; }
        public decimal Cost { get; set; }
         
        public List<Factor> Factors { get; set; }

        public List<Order> Orders { get; set; }


    }
}
