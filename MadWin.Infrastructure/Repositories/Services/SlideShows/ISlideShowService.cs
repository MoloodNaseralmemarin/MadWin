using MadWin.Core.DTOs;
using MadWin.Core.Entities.Slideshows;
using Microsoft.AspNetCore.Http;
using Shop2City.Core.DTOs;

namespace Shop2City.Core.Services.SlideShows
{
   public  interface ISlideShowService
    {
        List<SlideShowViewModel> GetSlideShows();

        void AddSlideShow(SlideShow slideShow, IFormFile imgProduct);

        void UpdateSlideShow(SlideShow slideShow, IFormFile imgProduct);

        SlideShow GetSlideShowBySlideShowId(int slideShowId);
    }
}
