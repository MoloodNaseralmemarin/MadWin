
using MadWin.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Shop2City.WebHost.ViewComponents
{
    public class OrderSummaryComponent : ViewComponent
    {
        private readonly IOrderService _orderService;

        public OrderSummaryComponent(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int pageNumber = 1, int pageSize = 10)
        {
            var model = await _orderService.GetTodayOrdersAsync(pageNumber, pageSize);
            return await Task.FromResult((IViewComponentResult)View("OrderSummary", model));
        }
    }
}
