using System;
using System.Collections.Generic;
using System.Text;

namespace MadWin.Core.DTOs
{
   public class SlideShowViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Alt { get; set; }

        public string FileName { get; set; }

        public DateTime? startShowDateTime { get; set; }

        public DateTime? endShowDateTime { get; set; }

        public int Sort { get; set; }

        public string Link { get; set; }
    }
}
