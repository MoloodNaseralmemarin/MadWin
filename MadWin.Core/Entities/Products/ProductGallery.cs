using MadWin.Core.Entities.Products;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MadWin.Core.Entities.Products
{
    [Table("ProductGalleries", Schema = "Production")]
    public class ProductGallery
    {
        [Key]
        [Display(Name = "شناسه")]
        public int ProductGalleryId { get; set; }

        public int ProductId { get; set; }

        [Display(Name = "تصویر محصول")]
        public string FileName { get; set; }

        [Display(Name = "عنوان عکس")]
        public string Title { get; set; }


        [Display(Name = "متن جایگزین")]
        public string Alt { get; set; }

        public bool isDelete { get; set; }


        #region Relationship
        public Product Product { get; set; }
        #endregion
    }
}
