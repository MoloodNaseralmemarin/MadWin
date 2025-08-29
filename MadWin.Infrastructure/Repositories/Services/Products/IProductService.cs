using MadWin.Core.DTOs.Products;
using MadWin.Core.Entities.Products;
using MadWin.Core.Entities.Properties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Shop2City.Core.Services.Products
{
    public interface IProductService
    {
        #region Products
        #region Index
        ProductForAdminViewModel GetProductForAdmin(
            int pageId = 1,
            string filterSerialNumber = "",
            string filterTitleProduct = "");
        #endregion
        #region Add
        int AddProduct(Product product, string username);
        #endregion
        #region common
        Product GetProductByProductId(int productId);
        #endregion
        #region Edit
        void UpdateProduct(Product product, int userId);
        #endregion
        #region Delete 
        void DeleteProduct(Product product, int userId);
        #endregion
        #endregion
        #region Filter
        public Tuple<List<ShowProductListItemViewModel>, int> GetProduct(int pageId = 1,
          string filterProductTitle = "",
          List<int> selectedGroups = null,
          int take = 0);
        #endregion

        #region EditPriceProduct


        void UpdatePriceProduct(List<int> newPrice, List<int> productId);

        #endregion

        Product GetProductForShow(int productId);

        /// <summary>
        /// Gets all Products displayed on home page
        /// </summary>
        /// <returns></returns>
        /// <returns>Products</returns>
        IList<ShowProductListItemViewModel> GetAllProductsDisplayedOnHomepage();

        /// <summary>
        /// Gets all Products bestsellers on home page
        /// </summary>
        /// <returns></returns>
        /// <returns>Products</returns>
        IList<ShowProductListItemViewModel> GetAllBestsellersProduct();

        /// <summary>
        /// Gets all Products bestsellers on ByGroupId
        /// </summary>
        /// <returns></returns>
        /// <returns>Products</returns>
        IList<ShowProductListItemViewModel> GetAllProductsByGroupId(int groupId);


        IList<Product> GetAllProduct();

        #region ProductGroups

        List<ProductGroup> GetAllGroup();

        List<ProductGroupsViewModel> ShowMainProductGroups();

        #region Categories

        List<SelectListItem> GetGroupForManageProduct();

        List<SelectListItem> GetCategoryForManageProduct(int groupId);

        List<SelectListItem> GetSubCategoryForManageProduct(int categoryId);

        #endregion


        ProductGroup GetProductGroupById(int productGroupId);


        ProductGroup GetProductGroupById(int productGroupId,int subproductGroupId);



        void UpdateProductGroups(ProductGroup productGroup);

        void AddProductGroups(ProductGroup productGroup);

        void DeleteProductGroups(ProductGroup productGroup);

        #endregion













       



        Product GetDetailsProductByProductId(int productId);
        #region ProductProperty

        //*
        //int AddProductProperty(ProductProperty productProperty);

        //*
        bool CheckExistFile(string fileName);

        //*
        List<ProductProperty> GetProductProperties(int productId);





        ProductProperty GetProductPropertyById(int productPropertyId);


        #endregion




        IEnumerable<Product> GetAll();

        Product GetById(int id);

        ProductForAdminViewModel GetDeleteProduct(int pageId = 1, string filterTitleProduct = "", int count = 0);

        ProductDetailsViewModel GetDetailProduct(int productId);

        List<Product> GetProductByProductCategory(int ProductCategory);
        int ProductCount();

        int ProductFactor();

        #region ProductImage
        ProductGallery GetProductImageByProductId(int ProductId);

        List<ProductGallery> GetProductActiveGalleries(int productId);
        #endregion
        Task<Product> GetProductByCategory(int productCategory, int productSubCategory);

    }
}
