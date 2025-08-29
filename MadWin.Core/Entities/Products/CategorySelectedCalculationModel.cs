using MadWin.Core.Entities.Common;
using MadWin.Core.Entities.CurtainComponents;

namespace MadWin.Core.Entities.Products
{
    public class CategorySelectedCalculationModel : BaseEntity
    {
        #region Properties

        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public int CalculationId { get; set; }

        #endregion

        #region Relations

        public CurtainComponent CurtainComponents { get; set; }

        #endregion
    }
}
