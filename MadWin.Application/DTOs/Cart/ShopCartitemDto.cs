namespace MadWin.Application.DTOs.Cart
{
    public class ShopCartitemDto
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }

        public string ImageName { get; set; }

        // جمع قیمت برای این محصول
        public decimal Sum => Price * Count;
    }

}
