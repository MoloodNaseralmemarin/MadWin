using MadWin.Core.Entities.CurtainComponents;
using MadWin.Core.Interfaces;
using MadWin.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Infrastructure.Repositories
{
    public class CurtainComponentDetailRepository:GenericRepository<CurtainComponentDetail>, Core.Interfaces.ICurtainComponentDetailRepository
    {
        private readonly MadWinDBContext _context;
        public CurtainComponentDetailRepository(MadWinDBContext context):base(context) 
        {
            _context = context;
        }
    }
}
