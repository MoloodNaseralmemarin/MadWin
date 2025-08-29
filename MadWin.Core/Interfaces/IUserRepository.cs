using MadWin.Core.Entities.Users;
using MadWin.Core.Lookups.Account;

namespace MadWin.Core.Interfaces
{
    public interface IUserRepository:IGenericRepository<User>
    {
        Task<bool> IsExistCellPhoneAsync(string cellPhone);
        Task<bool> IsExistUserNameAsync(string userName);

        Task<int> GetUserIdByUserName(string userName);

        Task<UserInfoLookup> GetUserInfoByUserName(string userName);
        Task<User> GetUserByUserId(string userName);

        Task<User?> GetUserIdByUserId(int userId);

        Task<User?> GetUserByUsernameAsync(string username);

        Task<string> GetCellPhoneByUserIdAsync(int userId);
    }
}
