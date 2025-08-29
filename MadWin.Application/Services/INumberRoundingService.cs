using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Application.Services
{
    public interface INumberRoundingService
    {
        int RoundLastThreeDigitsToZero(decimal number);
    }
}
