using MadWin.Core.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace MadWin.Core.Entities.Slideshows
{
    public class SlideShow :BaseEntity
    {
        #region ctor
        public SlideShow()
        {

        }
        #endregion
        #region Field

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "متن جایگزین")]
        public string Alt { get; set; }

        [Display(Name = "تصویر جایگزین")]
        public string FileName { get; set; }

        [Display(Name = "مسیر")]
        public string Path { get; set; }

        [Display(Name = "لینک")]
        public string Link { get; set; }

        [Display(Name = "تایخ شروع نمایش")]
        public DateTime? StartShowDateTime { get; set; }

        [Display(Name = "تایخ عدم نمایش")]
        public DateTime? EndShowDateTime { get; set; }

        [Display(Name = "ترتیب نمایش")]
        public int Sort { get; set; }

        [Display(Name = "فعال/غیر فعال")]
        public bool IsActive { get; set; }
        #endregion
    }
}
