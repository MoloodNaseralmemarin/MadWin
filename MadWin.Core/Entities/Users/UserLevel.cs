using MadWin.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWin.Core.Entities.Users
{
    public class UserLevel:BaseEntity
    {
    public UserLevel() { }
         public string Title { get; set; }

        [Display(Name = "درصد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int LevelPercent { get; set; }

    }
}
