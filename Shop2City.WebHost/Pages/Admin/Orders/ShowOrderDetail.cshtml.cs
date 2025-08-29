using MadWin.Application.Services;
using MadWin.Core.Entities.Orders;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Shop2City.Web.Pages.Admin.Orders
{
    public class ShowOrderDetailModel : PageModel
    {
        private readonly IOrderService _orderService;
        public ShowOrderDetailModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public string CustomerName { get; set; }
        public string ProductName { get; set; }

        public void OnGet(int id)
        {

        }
    }
}