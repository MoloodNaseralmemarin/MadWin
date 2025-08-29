using MadWin.Core.Common;
using System.ComponentModel.DataAnnotations;

namespace Shop2City.WebHost.ViewModels.Account
{
    #region ForgotPasswordViewModel
    public class ForgotPasswordViewModel
    {
        [Display(Name = "نام کاربری", Prompt = "نام کاربری")]
        [MaxLength(100, ErrorMessage = ErrorMessage.MaxLength)]
        [Required(ErrorMessage = ErrorMessage.Required)]
        public string UserName { get; set; }
    }
    #endregion
}
