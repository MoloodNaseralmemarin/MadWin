using MadWin.Core.Entities.SentMessages;
using MadWin.Core.Entities.Users;
using MadWin.Core.Interfaces;
using MadWin.Core.Lookups.Account;
using MadWin.Core.Lookups.CommissionRates;
using MadWin.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly MadWinDBContext _context;

        public UserRepository(MadWinDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> GetUserIdByUserName(string userName)
        {
            var user = await _context.Users.SingleAsync(u => u.UserName == userName);
            return user.Id;
        }


        public async Task<bool> IsExistCellPhoneAsync(string cellPhone)
        {
            return await _context.Users.AnyAsync(u => u.CellPhone == cellPhone);
        }

        public async Task<bool> IsExistUserNameAsync(string userName)
        {
            return await _context.Users.AnyAsync(u => u.UserName == userName);
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            if (userName == null) return null;
            var getUserId =await _context.Users
                .SingleOrDefaultAsync(u => u.UserName == userName);
            return getUserId ?? null;

        }

        public async Task<User> GetUserByUserId(int userId)
        {
            return await _context.Users
                .SingleOrDefaultAsync(u => u.Id == userId);
        }

        public Task<User> GetUserByUserId(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetUserIdByUserId(int userId)
        {
            return await _context.Users
                           .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<UserInfoLookup> GetUserInfoByUserName(string userName)
        {
            var item = await _context.Set<User>()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName == userName);

            if (item == null)
                return null;

            return new UserInfoLookup
            {
                UserId = item.Id,
                UserName = item.UserName,
                Password=item.Password
            };
        }

        public async Task<string> GetCellPhoneByUserIdAsync(int userId)
        {
            var user = await _context.Users.SingleAsync(u => u.Id == userId);
            return user.CellPhone;
        }
    }
}
