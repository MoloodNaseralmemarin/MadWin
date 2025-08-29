using MadWin.Core.DTOs.Calculations;
using MadWin.Core.Entities.CurtainComponents;
namespace MadWin.Core.Interfaces
{
    public interface ICurtainComponentProductGroupRepository :IGenericRepository<CurtainComponentProductGroup>
    {
        Task<IEnumerable<CurtainComponentProductGroupLookup>> CalculationByCategory(int categoryId, int subCategoryId);
    }
}
