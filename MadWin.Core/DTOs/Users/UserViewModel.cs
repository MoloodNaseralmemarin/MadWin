using MadWin.Core.Common;
using MadWin.Core.Entities.Users;
using System.ComponentModel.DataAnnotations;


namespace MadWin.Core.DTOs.Users
{
    #region UserForAdminViewModel
    public class UserForAdminViewModel
    {
        public List<User> users { get; set; }

        public int currentPage { get; set; }

        public int countPage { get; set; }
    }
    #endregion
    #region CreateUserViewModel
    public class CreateUserViewModel
    {
        [Display(Name = "نام", Prompt = "نام")]
        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(100, ErrorMessage = ErrorMessage.MaxLength)]
        public string firstName { get; set; }

        [Display(Name = "نام خانوادگی", Prompt = "نام خانوادگی")]
        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(100, ErrorMessage = ErrorMessage.MaxLength)]
        public string lastName { get; set; }

        [Display(Name = "شماره همراه", Prompt = "شماره همراه")]
        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(11, ErrorMessage = ErrorMessage.MaxLength)]
        public string cellPhone { get; set; }

        [Display(Name = "شماره ثابت", Prompt = "شماره ثابت")]
        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(11, ErrorMessage = ErrorMessage.MaxLength)]
        public string tellPhone { get; set; }


        [Display(Name = "نام کاربری", Prompt = "نام کاربری")]
        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(100, ErrorMessage = ErrorMessage.MaxLength)]
        public string? userName { get; set; }

        [Display(Name = "کلمه عبور", Prompt = "کلمه عبور")]
        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(100, ErrorMessage = ErrorMessage.MaxLength)]
        public string password { get; set; }

 

        [Display(Name = "آدرس پستی",Prompt ="آدرس پستی")]
        [MaxLength(500, ErrorMessage = ErrorMessage.MaxLength)]
        public string address { get; set; }

    }
    #endregion

    #region EditUserViewModel
    public class EditUserViewModel
    {
        public int userId { get; set; }

        [Display(Name = "نام کاربری", Prompt = "نام کاربری")]
        public string? userName { get; set; }


        [Display(Name = "نام", Prompt = "نام")]
        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(100, ErrorMessage = ErrorMessage.MaxLength)]
        public string firstName { get; set; }


        [Display(Name = "نام خانوادگی", Prompt = "نام خانوادگی")]
        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(100, ErrorMessage = ErrorMessage.MaxLength)]
        public string lastName { get; set; }


        [Display(Name = "شماره همراه", Prompt = "شماره همراه")]
        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(11, ErrorMessage = ErrorMessage.MaxLength)]
        public string cellPhone { get; set; }


        [Display(Name = "شماره ثابت", Prompt = "شماره همراه")]
        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(11, ErrorMessage = ErrorMessage.MaxLength)]
        public string tellPhone { get; set; }


        [Display(Name = "آدرس پستی")]
        [MaxLength(500, ErrorMessage = ErrorMessage.MaxLength)]
        public string address { get; set; }


        [Display(Name = "کلمه عبور", Prompt = "کلمه عبور")]
        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(100, ErrorMessage = ErrorMessage.MaxLength)]
        public string password { get; set; }

        public List<int> userRoles { get; set; }
    }
    #endregion

}
