using MadWin.Core.Entities.Products;
using MadWin.Core.Entities.Properties;

namespace MadWin.Core.DTOs.Products
{


    #region ProductForAdminViewModel
    public class ProductForAdminViewModel
    {
        public List<Product> Products { get; set; }

        public int CurrentPage { get; set; }

        public int PageCount { get; set; }
    }
    #endregion

    //Use
    #region productoverviewViewModel
    public class ShowProductListItemViewModel
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string ShortDescription { get; set; }

        public decimal Price { get; set; }

        public string FileName { get; set; }

        public bool IsStatus { get; set; }

    }
    #endregion

    #region InformationProductForShowViewModel

    public class InformationProductForShowViewModel
    {
        public int productId { get; set; }

        public string Title { get; set; }

        public string productTitleEn { get; set; }

        public int star { get; set; }

        public string ratingAvg { get; set; }

        public string productWarrantyTitle { get; set; }

        public string productDescription { get; set; }

        public string producteEvaluation { get; set; }

        public int productePrice { get; set; }

        public bool isFavorite { get; set; }

        public bool isNotification { get; set; }

        public List<ProductProperty> ProductProperties { get; set; }

        public List<ProductGallery> productImages { get; set; }

      




    }
    #endregion







    #region ProductGroupsViewModel

    public class ProductGroupsViewModel
    {
        public int ProductGroupId { get; set; }

        public string ProductGroupTitle { get; set; }

        public int ProductGroupQuantity { get; set; }
    }
    #endregion

 

    #region ProductPricesViewModel

    public class ProductPricesViewModel
    {
        public int productId { get; set; }

        public string productTitleFa { get; set; }

        public decimal oldPrice { get; set; }

        public int? newPrice { get; set; }


    }
    #endregion

    public class ProductDetailsViewModel
    {
        public int productId { get; set; }
        public string group { get; set; }
        public string category { get; set; }
        public string subCategory { get; set; }
        public string? title { get; set; }
        public string shortDescription { get; set; }

        public string text { get; set; }

        public int price { get; set; }

        public List<ProductImageViewModel>? listProductImage { get; set; }
        public List<PropertyProductViewModel>? listPropertyProduct { get; set; }
    }

    public class PropertyProductViewModel
    {
        public string? Title { get; set; }

        public string? Value { get; set; }
    }

    public class ProductImageViewModel
    {
        public string? imageUrl { get; set; }
        public string? alt { get; set; }
        public string? title { get; set; }
    }
}
