using MadWin.Core.Entities.Common;
using System.ComponentModel.DataAnnotations;


namespace MadWin.Core.Entities.CurtainComponents
{
    public class CurtainComponent:BaseEntity
    {
        [Display(Name="عنوان")]
        public string Name { get; set; }
        [Display(Name = "قیمت")]
        public decimal Cost { get; set; }


        public virtual List<CurtainComponentDetail> CurtainComponentDetails { get; set; }


    }
}
