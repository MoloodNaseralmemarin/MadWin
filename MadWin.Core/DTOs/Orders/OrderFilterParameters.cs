namespace MadWin.Core.DTOs.Orders
{
    public class OrderFilterParameters
    {
        public string FullName { get; set; }
        public int? OrderId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? FromPrice { get; set; }
        public int? ToPrice { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;

    }
}
