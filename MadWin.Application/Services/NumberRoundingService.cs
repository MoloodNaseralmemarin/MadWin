using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Application.Services
{
    public class NumberRoundingService : INumberRoundingService
    {
        public int RoundLastThreeDigitsToZero(decimal number)
        {
            var result =(int) Math.Floor(number / 1000) * 1000;
            return result;
        }
    }
}
