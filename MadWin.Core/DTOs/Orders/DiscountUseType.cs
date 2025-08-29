using System;
using System.Collections.Generic;
using System.Text;

namespace MadWin.Core.DTOs.Orders
{
    public enum DiscountUseType
    {
        Success ,
        ExpirationDate,
        NotFound,
        Finished,
        UserUsed
    }
}
