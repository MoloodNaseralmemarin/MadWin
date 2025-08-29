using MadWin.Core.Entities.CurtainComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Core.Interfaces
{
    public interface ICurtainComponentRepository:IGenericRepository<CurtainComponent>
    {
        Task<decimal> GetPriceByIdAsync(int id);
    }
}
