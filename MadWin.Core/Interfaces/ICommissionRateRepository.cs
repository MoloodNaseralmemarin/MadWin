using MadWin.Core.Entities.CommissionRates;
using MadWin.Core.Lookups.CommissionRates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Core.Interfaces
{
    public interface ICommissionRateRepository:IGenericRepository<CommissionRate>
    {
        Task<CommissionInfoLookup> GetCommissionInfoAsync(int partCount, bool isEqualParts);
    }
}
