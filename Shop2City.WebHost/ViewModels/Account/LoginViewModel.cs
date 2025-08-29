using MadWin.Core.Common;
using System.ComponentModel.DataAnnotations;

namespace Shop2City.WebHost.ViewModels.Account
{
    #region LoginViewModel
    public class LoginViewModel
    {
        [Display(Name = "شماره همراه", Prompt = "شماره همراه")]
        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(11, ErrorMessage = ErrorMessage.MinLength)]
        public string userName { get; set; }

        [Display(Name = "کلمه عبور", Prompt = "کلمه عبور")]
        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(100, ErrorMessage = ErrorMessage.MaxLength)]
        public string password { get; set; }

        [Display(Name = "مرا را به خاطر بسپار")]
        public bool rememberMe { get; set; }
    }
    #endregion

}
