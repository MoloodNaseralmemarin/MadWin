using MadWin.Core.DTOs.Factors;
using MadWin.Core.Entities.Factors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Application.Services
{
    public interface IFactorDetailService
    {
        Task<FactorSummaryDto> GetFactorSummaryByFactorIdAsync(int factorId);

        Task<List<FactorDetail>> GetAllFactorDetailByFactorIdAsync(int factorId);

        Task SoftDeleteAsync(IEnumerable<int> factorDetailIds);

    }
}
