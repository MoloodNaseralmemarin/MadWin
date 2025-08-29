

using MadWin.Infrastructure.Context;
using MadWin.Core.Entities.Users;

namespace Shop2City.Core.Services.UserRoles
{
    public class UseRoleService : IUseRoleService
    {
        private readonly MadWinDBContext _context;

        public UseRoleService(MadWinDBContext context)
        {
            _context = context;
        }
        public int AddUseRole(UserRole userRole)
        {
            _context.UserRoles.Add(userRole);
            _context.SaveChanges();
            return userRole.userRoleId;
        }
    }
}
