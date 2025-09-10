using MadWin.Core.DTOs.Products;
using MadWin.Infrastructure.Context;
using MadWin.Core.Entities.Products;
using MadWin.Core.Entities.Properties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop2City.Core.Generator;
using MadWin.Application.Services;

namespace Shop2City.Core.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IUserService _userService;
        private readonly MadWinDBContext _context;

        public ProductService(IUserService userService, MadWinDBContext context)
        {
            _userService = userService;
            _context = context;
        }

        #region Product
        #region Index
        public ProductForAdminViewModel GetProductForAdmin(
         int pageId = 1,
         string filterSerialNumber = "",
         string filterProductTitleFa = "")
        {
            IQueryable<Product> result = _context.Products;
            #region filterProduct
            if (!string.IsNullOrEmpty(filterProductTitleFa))
            {
                result = result.Where(u => u.Title.Contains(filterProductTitleFa));
            }
            #endregion

            #region Paging
            var take = 10; // تعداد نمایش اطلاعات در هر صفحه
            var skip = (pageId - 1) * take;
            var list = new ProductForAdminViewModel
            {
                CurrentPage = pageId,
                PageCount = (int)Math.Ceiling(decimal.Divide(result.Count(), take)),
                Products = result
                    .Skip(skip)
                    .Take(take)
                    .ToList()
            };
            #endregion
            return list;
        }
        #endregion
        #region Add
        public async Task<int> AddProduct(Product product, string username)
        {
            var userId =await _userService.GetUserIdByUserName(username);

            product.UserId = userId;

            _context.Products.Add(product);
            _context.SaveChanges();
            return product.Id;
        }
        #endregion
        #region Common
        public Product GetProductByProductId(int productId)
        {
            return _context.Products.Find(productId);
        }
        #endregion
        #region Edit
        public void UpdateProduct(Product product,int userId)
        {
            product.UserId = userId;
            product.LastUpdateDate = DateTime.Now;
            _context.Products.Update(product);
            _context.SaveChanges();

        }
        #endregion
        #region Delete
        public void DeleteProduct(Product product, int userId)
        {
            product.UserId = userId;
            product.IsDelete = true;
            UpdateProduct(product, userId);
        }
        #endregion
        #endregion


 

        public void AddProductGroups(ProductGroup productGroup)
        {
            productGroup.Description = "non";
            productGroup.IsActive = true;
            _context.ProductGroups.Add(productGroup);
            _context.SaveChanges();
        }

        public int AddProductProperty(ProductProperty productProperty)
        {
            _context.ProductProperties.Add(productProperty);
            _context.SaveChanges();
            return productProperty.PropertyProductId;
        }



        public bool CheckExistFile(string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/products/images", fileName);
            return File.Exists(path);
        }

  



        public void detailProductProperty(ProductProperty productProperty)
        {
            _context.ProductProperties.Update(productProperty);
            _context.SaveChanges();
        }

        public virtual IList<ShowProductListItemViewModel> GetAllBestsellersProduct()
        {
            return _context.Products
                .Take(5)
                .Select(p => new ShowProductListItemViewModel
                {
                    Id = p.Id,
                    //productPrice = p.productPrice,
                    Title = p.Title,
                })
                .ToList();
        }


        public List<SelectListItem> GetGroupForManageProduct()
        {
            return _context.ProductGroups
                .Where(g => g.ParentId == null && g.IsActive)
                .Select(g => new SelectListItem()
                {
                    Text = g.Title,
                    Value = g.Id.ToString()
                })
                .ToList();
        }
        public void UpdateProduct()
        {
            throw new NotImplementedException();
        }


        public Product GetDetailsProductByProductId(int productId)
        {
            var product = GetProductByProductId(productId);
            var details = new Product
            {
                Title = product.Title,
                //new ProductWarranty { ProductWarrantyTitleFa=product.ProductWarranty.ProductWarrantyTitleFa}
            };


            return product;
        }



        public Product GetProductForShow(int productId)
        {
            return _context.Products
                .Include(p => p.ProductGalleries)
                //.Include(p => p.ProductProperties).ThenInclude(p => p.Property)
                //.Include(p => p.PropertyTechnicalProducts).ThenInclude(p => p.PropertyTechnical)
                .SingleOrDefault(p => p.Id == productId);
        }

        public List<ProductGallery> GetProductActiveGalleries(int productId)
        {
            return  _context.ProductGalleries
                .Where(pg => pg.ProductId == productId && !pg.isDelete)
                .Select(pg => new ProductGallery
                {
                    ProductId = pg.ProductId,
                    ProductGalleryId = pg.ProductGalleryId,
                    FileName = pg.FileName
                }).ToList();
        }

        public ProductGroup GetProductGroupById(int productGroupId)
        {
            return _context.ProductGroups
                .Find(productGroupId);
        }

        public List<ProductGroup> GetAllGroup()
        {
            return _context.ProductGroups
                .Where(p=>p.IsShowMenu==true)
                .ToList();
        }

        public List<ProductProperty> GetProductProperties(int productId)
        {
            return _context
                .ProductProperties
                .Where(pp => pp.ProductId == productId)
                .ToList();
        }

        public ProductProperty GetProductPropertyById(int productPropertyId)
        {
            return _context.ProductProperties
                .Find(productPropertyId);
        }

        public List<SelectListItem> GetCategoryForManageProduct(int groupId)
        {
            return _context.ProductGroups
                .OrderBy(pg => pg.Title)
                .Where(pg => pg.ParentId == groupId && pg.IsActive)
                .Select(pg => new SelectListItem
                {
                    Text = pg.Title,
                    Value = pg.Id.ToString()
                }).OrderByDescending(a=>a.Value)
                .ToList();
        }

        public List<SelectListItem> GetSubCategoryForManageProduct(int categoryId)
        {
            return _context.ProductGroups
                .Where(pg => pg.ParentId == categoryId && pg.IsActive)
                .Select(pg => new SelectListItem
                {
                    Text = pg.Title,
                    Value = pg.Id.ToString()
                })
                .ToList();
        }

        public void UpdateProduct(Product product, IFormFile imgProduct)
        {
            product.LastUpdateDate = DateTime.Now;
            product.UserId = 2;


            _context.Products.Update(product);
            _context.SaveChanges();

        }





        public void DeleteProductGroups(ProductGroup productGroup)
        {
            productGroup.IsDelete = true;
            UpdateProductGroups(productGroup);
        }

        public InformationProductForShowViewModel GetShowProduct(int productId)
        {
            var product = GetProductByProductId(productId);

            var informationProduct = new InformationProductForShowViewModel
            {
                productId = productId,
                Title = product.Title,
                //isFavorite=

                //productePrice = ProductePrices.OrderByDescending(a => a.ProductId).FirstOrDefault().Price,
                // ratingAvg = product.ProductRatings.Sum(pr=>pr.ProductRank)/product.ProductRatings.quantity(),
                // isFavorite = User.Identity.IsAuthenticated ? (isFavorite.Where(a => a.UserID == User.GetUserID()).SingleOrDefault() != null ? true : false) : false;
            };

            return informationProduct;
        }

        public ProductForAdminViewModel GetDeleteProduct(int pageId = 1, string filterTitleProduct = "", int count = 0)
        {
            IQueryable<Product> result = _context.Products
                .OrderByDescending(p => p.CreateDate)
                .IgnoreQueryFilters();

            if (!string.IsNullOrEmpty(filterTitleProduct))
            {
                result = result
                    .Where(p => p.Title.Contains(filterTitleProduct));
            }

            #region Show Item In Page

            int take = 50;
            int skip = (pageId - 1) * take;

            #endregion

            ProductForAdminViewModel list = new ProductForAdminViewModel
            {
                CurrentPage = pageId,
                PageCount = result
                    .Count() / take,
                Products = result
                    .OrderBy(u => u.CreateDate)
                    .Skip(skip).Take(take)
                    .ToList()
            };
            return list;
        }

        public ProductGallery GetProductImageByProductId(int productId)
        {
            var s= _context.ProductGalleries.Where(a => a.ProductId == productId)
                .SingleOrDefault();
            return s;
        }


        public IList<ShowProductListItemViewModel> GetAllProductsByGroupId(int groupId)
        {
            return _context.Products
                .Where(p => p.SubCategoryId == groupId)
                .Take(8)
                .Select(p => new ShowProductListItemViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    FileName =p.ProductGalleries!.OrderByDescending(pi => pi.ProductGalleryId).Last().FileName,
                    Price=p.Price,
                    IsStatus = p.IsStatus,
                })
                .ToList();
        }

        public void UpdateProductImage(ProductGallery productImage, IFormFile imgProduct)
        {
            if (imgProduct != null)
            {
                if (productImage.FileName != null)
                {
                    string deleteimagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/products/images",
                        productImage.FileName);
                    if (File.Exists(deleteimagePath))
                    {
                        File.Delete(deleteimagePath);
                    }

                    string deletethumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/products/thumbs",
                        productImage.FileName);
                    if (File.Exists(deletethumbPath))
                    {
                        File.Delete(deletethumbPath);
                    }
                }

                productImage.FileName =
                    NameGenerator.GenerateUniqCode() + Path.GetExtension(imgProduct.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/products/images",
                    productImage.FileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgProduct.CopyTo(stream);
                }

                //ImageConvertor imgResizer = new ImageConvertor();
                //string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/products/thumbs",
                //    productImage.productImageName);

                //imgResizer.Image_resize(imagePath, thumbPath, 150);
            }

            //if (productDemo != null)
            //{
            //    if (product.ProductDemoFileName != null)
            //    {
            //        string deleteDemoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/products/demoes", product.ProductDemoFileName);
            //        if (File.Exists(deleteDemoPath))
            //        {
            //            File.Delete(deleteDemoPath);
            //        }
            //    }
            //    product.ProductDemoFileName = NameGenerator.GenerateUniqCode() + Path.GetExtension(productDemo.FileName);
            //    string demoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/products/demoes", product.ProductDemoFileName);
            //    using (var stream = new FileStream(demoPath, FileMode.Create))
            //    {
            //        productDemo.CopyTo(stream);
            //    }
            //}
            _context.ProductGalleries.Update(productImage);
            _context.SaveChanges();
        }

        public void DeleteProductImage(ProductGallery productImage)
        {
            UpdateProductImage(productImage);
        }

        public void UpdateProductImage(ProductGallery productImage)
        {
            productImage.isDelete = true;
            _context.ProductGalleries.Update(productImage);
            _context.SaveChanges();
        }

        public ProductGallery GetProductImageByProductImageId(int productImageId)
        {
            return _context.ProductGalleries
                //  .Include(p => p.Product)
                .FirstOrDefault(m => m.ProductGalleryId == productImageId);
        }

        public void UpdateProductGroups(ProductGroup productGroup)
        {
            productGroup.Description = "non";
            productGroup.IsActive = true;
            _context.ProductGroups.Update(productGroup);
            _context.SaveChanges();
        }
        public List<ProductGroupsViewModel> ShowMainProductGroups()
        {
            return _context.ProductGroups
                .Where(pg => pg.ParentId == null)
                .Select(p => new ProductGroupsViewModel
                {
                    ProductGroupId = p.Id,
                    ProductGroupTitle = p.Title,
                    ProductGroupQuantity = p.ProductGroups.Count()

                })
                .ToList();
        }

        public IList<ShowProductListItemViewModel> GetAllProductsDisplayedOnHomepage()
        {
            return _context.Products
                
                .Select(p => new ShowProductListItemViewModel
                {
                    Id = p.Id,
                    Title = p.Title
                })
                .ToList();
        }

        public List<ProductPricesViewModel> GetListProductPrices()
        {
            return _context.Products
                .Select(p => new ProductPricesViewModel
                {
                    productId = p.Id,
                    productTitleFa = p.Title,
                })
                .ToList();
        }

        public void UpdatePriceProduct(List<int> newPrice, List<int> productId)
        {
            _context.SaveChanges();
        }

 

        public IList<Product> GetAllProduct()
        {
            return _context.Products.ToList();
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products
                .Include(p => p.Category);
        }

        public Product GetById(int id)
        {
            return GetAll()
                .FirstOrDefault(p => p.Id == id);
        }


 

        public ProductDetailsViewModel GetDetailProduct(int productId)
        {

            var product = GetProductForShow(productId);
            ProductDetailsViewModel productDetails = new ProductDetailsViewModel()
            {
                productId = product.Id,
                title = product.Title
            };
            return productDetails;

        }

        public List<Product> GetProductByProductCategory(int ProductCategory)
        {
            var qProdcut = _context.Products.Where(a => a.CategoryId == ProductCategory)
                .ToList();
            return qProdcut;
        }
        public int ProductCount()
        {
            return _context.Products.Count();
        }

        public int ProductFactor()
        {
            return _context.Factors.Count();
        }
        public List<ProductProperty> GetListPropertyProduct(int productId)
        {
            return _context
                .ProductProperties
                .Where(pi => pi.ProductId == productId)
                .ToList();
        }

        public void AddPropertyProduct(ProductProperty propertyProduct)
        {

            _context.ProductProperties.Add(propertyProduct);
            _context.SaveChanges();
        }

        public ProductProperty GetPropertyProductByPropertyProductId(int propertyProductId)
        {
            return _context.ProductProperties.Find(propertyProductId);
        }

        public void UpdatePropertyProduct(ProductProperty propertyProduct)
        {
            _context.ProductProperties.Update(propertyProduct);
            _context.SaveChanges();
        }

        public void DeletePropertyProduct(ProductProperty propertyProduct)
        {

            _context.ProductProperties.Update(propertyProduct);
            _context.SaveChanges();
        }

        public List<ProductProperty> ListPropertyProductByProductId(int productId)
        {
            var getListPropertyProduct = _context.ProductProperties
                .Include(pi => pi.Product)
                .Where(a => a.ProductId == productId)
                .ToList();
            return getListPropertyProduct;
        }



        public void UpdateProduct(int productId)
        {
            var product = GetProductByProductId(productId);
            product.IsDelete = true;
            product.LastUpdateDate = DateTime.Now;
            _context.Update(product);
            _context.SaveChanges();
        }


        public async Task<Product> GetProductByCategory(int productCategory, int productSubCategory)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.CategoryId == productCategory && p.SubCategoryId == productSubCategory);
            return product;
        }

        public ProductGroup GetProductGroupById(int productGroupId, int subproductGroupId)
        {
            throw new NotImplementedException();
        }


        public Tuple<List<ShowProductListItemViewModel>, int> GetProduct(int pageId = 1,
          string filterProductTitle = "",
          List<int> selectedGroups = null,
          int take = 0)
        {
            if (take == 0)
                take = 40;
            var result = _context.Products
                .Where(p => !p.IsDelete);
            if (!string.IsNullOrEmpty(filterProductTitle))
            {
                result = result
                    .Where(p => p.Title.Contains(filterProductTitle) || p.Tags.Contains(filterProductTitle));
            }

            if (selectedGroups != null && selectedGroups.Any())
            {
                foreach (int groupId in selectedGroups)
                {
                    result = result.Where(p =>
                        p.CategoryId == groupId || p.CategoryId == groupId || p.SubCategoryId == groupId);
                }
            }

            var skip = (pageId - 1) * take;
            var pagequantity = (int)Math.Ceiling(decimal.Divide(result.Select(p => new ShowProductListItemViewModel()
            {
                Id = p.Id,
                Title = p.Title,
                FileName = p.ProductGalleries.OrderByDescending(pp => pp.ProductId).First().FileName,
                Price = p.Price,
                IsStatus=p.IsStatus

            })
                    .Count(),
                take));

            var query = result.Select(p => new ShowProductListItemViewModel()
            {
                Id = p.Id,
                Title = p.Title,
                FileName = p.ProductGalleries.OrderByDescending(pp => pp.ProductId).First().FileName,
                Price = p.Price,
                IsStatus = p.IsStatus
            })
                .Skip(skip)
                .Take(take)
                .ToList();
            return Tuple.Create(query, pagequantity);
        }

        int IProductService.AddProduct(Product product, string username)
        {
            throw new NotImplementedException();
        }
    }
}








