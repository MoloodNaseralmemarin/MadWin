using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MadWin.Core.Entities.Common;
using MadWin.Core.Entities.Orders;

namespace MadWin.Core.Entities.Products
{
    public class ProductGroup :BaseEntity
    {
        #region ctor
        public ProductGroup()
        {

        }
        #endregion
        #region Field

        [Display(Name = "گروه اصلی")]
        public int? ParentId { get; set; }

        [Display(Name = "عنوان ")]
        public string Title { get; set; }

        [Display(Name = "فعال/غیرفعال")]
        public bool IsActive { get; set; }

        [Display(Name = "نمایش/عدم نمایش")]
        public bool IsShowMenu { get; set; }
        #endregion
        #region Relationship

        [ForeignKey("parentId")]
        public List<ProductGroup> ProductGroups { get; set; } 


        [InverseProperty("ProductGroup")]
        public List<Product> Products { get; set; }


        [InverseProperty("Category")]
        public List<Product> Category { get; set; }


        [InverseProperty("SubCategory")]
        public List<Product> SubCategory { get; set; }


        #region order
        [InverseProperty("OrderCategory")]
        public List<Order> OrderCategory { get; set; }

        [InverseProperty("OrderSubCategory")]
        public List<Order> OrderSubCategory { get; set; }
        #endregion

        #endregion
    }
}
