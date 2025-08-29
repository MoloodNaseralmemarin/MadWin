using MadWin.Application.DTOs.Orders;
using MadWin.Core.Entities.CurtainComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Application.Services
{
    public interface ICurtainComponentDetailService
    {
        Task<CurtainComponentDetail> CreateCurtainComponentDetailInitialAsync(int orderId, int curtainComponentId, decimal unitCost, int count);
    }
}
