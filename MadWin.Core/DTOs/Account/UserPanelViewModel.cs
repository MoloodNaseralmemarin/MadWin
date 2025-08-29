using MadWin.Core.Common;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MadWin.Core.DTOs.Account
{
    #region InformationUserViewModel
    public class InformationUserViewModel
    {
        public string fullName { get; set; }
        public string? userName { get; set; }
        public string email { get; set; }
        public string cellPhone { get; set; }
        public DateTime registerDate { get; set; }
        public int Wallet { get; set; }
    }
    #endregion

    #region SideBarUserPanelViewModel
    public class SideBarUserPanelViewModel
    {
        public string imageName { get; set; }
        public string fullName { get; set; }
        public DateTime registerDate { get; set; }
    }
    #endregion

    #region EditProfileViewModel
    public class EditProfileViewModel
    {
        [Display(Name = "آدرس پستی")]
        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(100, ErrorMessage = ErrorMessage.MaxLength)]
        public string address { get; set; }

        [Display(Name = "شماره همراه")]
        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(100, ErrorMessage = ErrorMessage.MaxLength)]
        public string cellPhone { get; set; }

        [Display(Name = "تلفن ثابت")]
        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(100, ErrorMessage = ErrorMessage.MaxLength)]
        public string telPhone { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(100, ErrorMessage = ErrorMessage.MaxLength)]
        public string? userName { get; set; }

        [Display(Name = "پست الکترونیکی")]
        [MaxLength(100, ErrorMessage = ErrorMessage.MaxLength)]
        public string email { get; set; }

        [Display(Name = "تصویر پروفایل")]
        public IFormFile userAvatarFileName { get; set; }

        public string userAvatarImageName { get; set; }
    }
    #endregion

    #region ChangePasswordViewModel
    public class ChangePasswordViewModel
    { 
        [Display(Name = "کلمه عبور فعلی", Prompt = "کلمه عبور فعلی")]
        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(100, ErrorMessage = ErrorMessage.MaxLength)]
        public string currentPassword { get; set; }

        [Display(Name = "کلمه عبور جدید", Prompt = "کلمه عبور جدید")]
        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(100, ErrorMessage = ErrorMessage.MaxLength)]
        public string password { get; set; }

        [Display(Name = "تکرار کلمه عبور جدید", Prompt = "تکرار کلمه عبور جدید")]
        [Compare("password", ErrorMessage = ErrorMessage.Compare)]
        [MaxLength(100, ErrorMessage = ErrorMessage.MaxLength)]
        public string confirmPassword { get; set; }
    }
    #endregion

 

}
