using System.Collections.Generic;
using MadWin.Core.DTOs.Account;

namespace Shop2City.Core.Services.UserPanel
{
    public interface IUserPanelService
    {
        InformationUserViewModel GetInformationUser(string userName);

        Task<InformationUserViewModel> GetInformationUser(int userId);

        Task<SideBarUserPanelViewModel> GetSideBarUserPanelDataAsync(int id);

        EditProfileViewModel GetDataForEditProfileUser(string userName);

        void EditProfile(string userName, EditProfileViewModel editProfile);

        bool CompareOldPassword(string userName, string oldPassword);

        void ChangeUserPassword(string userName, string newPassword);

        
    }
}
