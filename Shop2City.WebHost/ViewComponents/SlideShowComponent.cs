using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Shop2City.Core.Services.SlideShows;

namespace Shop2City.Web.ViewComponents
{
    public class SlideShowComponent : ViewComponent
    {
        private readonly ISlideShowService _slideShowService;
        public SlideShowComponent(ISlideShowService slideShowServicet)
        {
            _slideShowService = slideShowServicet;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("SlideShow", _slideShowService.GetSlideShows()));
        }
    }
}