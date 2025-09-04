using MadWin.Core.DTOs.Factors;
using MadWin.Core.Entities.Factors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Core.Interfaces
{
    public interface IFactorDetailRepository:IGenericRepository<FactorDetail>
    {
        Task<FactorDetail> AddFactorDetailAsync(int factorId, int count, int productId);
        Task<decimal> FactorSum(int factorId);
        Task<bool> IsExistFactorDetailAsync(int factorId);
        Task<FactorDetail> GetFactorByfactorIdAsync(int factorId);
        Task<FactorSummaryDto> GetFactorSummaryByFactorIdAsync(int factorId);

        Task<List<FactorDetail>> GetAllFactorDetailByFactorIdAsync(int factorId);

        Task<FactorDetail?> GetFactorDetailByProductIdAsync(int factorId, int productId);

        decimal GetSubtotalByFactorId(int factorId);

    }
}
