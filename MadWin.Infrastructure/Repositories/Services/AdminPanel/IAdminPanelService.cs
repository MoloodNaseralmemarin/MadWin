using MadWin.Core.DTOs.Users;

namespace Shop2City.Core.Services.AdminPanel
{
    public interface IAdminPanelService
    {
        UserForAdminViewModel GetAllUsers(int pageId = 1, string filterFirstName = "", string filterLastName = "");
        UserForAdminViewModel GetDeleteUsers(int pageId = 1, string filterFirstName = "", string filterLastName = "");
        Task<int> AddUserFromAdmin(CreateUserViewModel createUser);
        EditUserViewModel GetUserForShowEditMode(int userId);
        void EditUserFromAdmin(EditUserViewModel editUser);
        void DeleteUserFromAdmin(int userId);
    }
}
