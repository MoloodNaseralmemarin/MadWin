using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MadWin.Core.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop2City.Core.DTOs;
using Shop2City.Core.Services.SlideShows;

namespace Shop2City.Web.Pages.Admin.SlideShows
{
    public class IndexModel : PageModel
    {
        private ISlideShowService _slideShowService;
        public IndexModel(ISlideShowService slideShowService)
        {
            _slideShowService = slideShowService;
        }

        public List<SlideShowViewModel> listSlideShows { get; set; }
        public void OnGet()
        {
            listSlideShows = _slideShowService.GetSlideShows();
        }
    }
}