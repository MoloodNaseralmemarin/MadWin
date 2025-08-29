using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MadWin.Core.Entities.Settings
{

    public class Setting
    {

        public Setting()
        {

        }
        public int settingId { get; set; }

        public string email { get; set; }

        public string cellPhone { get; set; }

        public string cellPhone1 { get; set; }

        public string telPhone { get; set; }

        public string telPhone1 { get; set; }

        public string logoImageName { get; set; }

        public string address { get; set; }

        public string copyRight { get; set; }

        public string siteTitleFa { get; set; }

        public string siteTitleEn { get; set; }

        public string siteDescription { get; set; }

        public string siteKeyWords { get; set; }

        public string siteFavIcon { get; set; }

        [Display(Name = "فقط نمایش نظرات تایید شده")]
        public bool onlyShowConfirmedComment { get; set; }


    }
}
