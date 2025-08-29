using MadWin.Core.Entities.Products;
using MadWin.Core.Entities.Users;
using System.ComponentModel.DataAnnotations;


namespace MadWin.Core.Entities.Products
{
    public class UserProduct
    {
        [Key]
        public int userProductId { get; set; }
        public int userId { get; set; }
        public int productId { get; set; }


        public Product Product { get; set; }
        public User User { get; set; }
    }
}
