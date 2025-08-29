using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MadWin.Core.Entities.Properties;
using MadWin.Core.Entities.Common;
using MadWin.Core.Entities.Users;
using MadWin.Core.Entities.Factors;


namespace MadWin.Core.Entities.Products
{
    [Table("Products", Schema = "Production")]
    public class Product:BaseEntity
    {
        #region ctor
        public Product()
        {
            CreateDate=DateTime.Now;
            LastUpdateDate=DateTime.Now;
            IsDelete = false;
        }
        #endregion
        #region Field

        [Display(Name = "کاربر ثبت کننده")]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "دسته اصلی")]
        public int ProductGroupId { get; set; }

        [Display(Name = "گروه اصلی")]
        public int? CategoryId { get; set; }

        [Display(Name = "گروه فرعی")]
        public int? SubCategoryId { get; set; }

        [Display(Name = "عنوان ( حداقل سه کلمه)")]
        public string? Title { get; set; }


        [Display(Name = "توضیح مختصر")]
        public string? ShortDescription { get; set; }


        [Display(Name = "توضیح کامل")]
        public string? FullDescription { get; set; }

        [Display(Name = "قیمت")]
    
        public int Price { get; set; }


        [Display(Name = "کلمه کلیدی")]
        public string? Tags { get; set; }

        [Display(Name = "موجود / ناموجود")]
        public bool IsStatus { get; set; }

        #endregion
        #region Relationship

        [ForeignKey("ProductGroupId")]
        public ProductGroup? ProductGroup { get; set; }
        [ForeignKey("CategoryId")]
        public ProductGroup? Category { get; set; }
        [ForeignKey("SubCategoryId")]
        public ProductGroup? SubCategory { get; set; }
        public User User { get; set; }
        public List<ProductGallery> ProductGalleries { get; set; }
        public List<ProductProperty>? ProductProperties { get; set; }

        public List<PropertyTechnicalProduct>? PropertyTechnicalProducts { get; set; }
        public virtual List<FactorDetail>? FactorDetails { get; set; }

        #endregion
    }
}

