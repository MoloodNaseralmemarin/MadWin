using MadWin.Application.DTOs.Account;
using MadWin.Core.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Application.Services
{
    public interface IUserService
    {
        Task<bool> IsExistCellPhoneAsync(string cellPhone);
        Task<bool> IsExistUserNameAsync(string userName);
        Task<int> GetUserIdByUserName(string userName);
        Task<User> GetUserByUserName(string userName);

        Task<User> GetUserIdByUserId(int userId);

        Task<LoginResultDto?> LoginAsync(LoginResultDto dto);

        Task CreateUser(User user);

        Task ExtractAndSaveLast4DigitsAsync(string userName);

        Task<string> GetCellPhoneByUserIdAsync(int userId);

    }
}
