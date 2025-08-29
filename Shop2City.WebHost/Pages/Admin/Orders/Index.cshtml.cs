using MadWin.Application.Services;
using MadWin.Core.DTOs.Orders;
using MadWin.Core.Entities.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Shop2City.Web.Pages.Admin.Orders
{
    public class IndexModel : PageModel
    {
        private readonly IOrderService _orderService;
        public IndexModel(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public PagedResult<OrderSummaryDto> Orders { get; set; }
        public OrderFilterParameters Filter { get; set; }

        public async Task OnGetAsync([FromQuery] OrderFilterParameters filter)
        {
            Orders = await _orderService.GetOrderSummaryAsync(filter);
            Filter = filter;
        }
    }
}