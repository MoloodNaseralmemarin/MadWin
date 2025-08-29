
using MadWin.Core.Entities.Common;
using MadWin.Core.Entities.CurtainComponents;

namespace MadWin.Core.Entities.CurtainComponents
{
    public class CurtainComponentProductGroup : BaseEntity
    {
        #region Properties

        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public int CurtainComponentId { get; set; }

        #endregion

        #region Relations

        public CurtainComponent Calculation { get; set; }

        #endregion
    }
}
