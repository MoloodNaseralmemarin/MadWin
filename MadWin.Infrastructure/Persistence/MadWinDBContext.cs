using MadWin.Core.Entities.Advices;
using MadWin.Core.Entities.CommissionRates;
using MadWin.Core.Entities.CurtainComponents;
using MadWin.Core.Entities.DeliveryMethods;
using MadWin.Core.Entities.Discounts;
using MadWin.Core.Entities.Factors;
using MadWin.Core.Entities.Orders;
using MadWin.Core.Entities.Permissions;
using MadWin.Core.Entities.Products;
using MadWin.Core.Entities.Properties;
using MadWin.Core.Entities.SentMessages;
using MadWin.Core.Entities.Settings;
using MadWin.Core.Entities.Slideshows;
using MadWin.Core.Entities.Transactions;
using MadWin.Core.Entities.Users;
using Microsoft.EntityFrameworkCore;


namespace MadWin.Infrastructure.Context
{
    public class MadWinDBContext : DbContext
    {
        public MadWinDBContext(DbContextOptions<MadWinDBContext> options) : base(options)
        {

        }

        public virtual DbSet<CurtainComponent> CurtainComponents { get; set; }

        #region Users Table
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<LoginHistory> LoginHistories { get; set; }
        public virtual DbSet<UserProduct> UserProducts { get; set; }
        public virtual DbSet<UserDiscountCode> UserDiscountCodes { get; set; }



        public virtual DbSet<UserLevel> UserLevels { get; set; }
        #endregion

        #region Products Tables
        public virtual DbSet<ProductGroup> ProductGroups { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductGallery> ProductGalleries { get; set; }

        public virtual DbSet<CategorySelectedCalculationModel> CategorySelectedCalculations { get; set; }
        #endregion

        #region Orders

        public virtual DbSet<Factor> Factors { get; set; }
        public virtual DbSet<FactorDetail> FactorDetails { get; set; }

        #endregion

        #region Orders
        public virtual DbSet<OrderWidthPart> OrderWidthParts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }

        #endregion
        public virtual DbSet<CurtainComponentDetail> CurtainComponentDetails { get; set; }

        public virtual DbSet<CurtainComponentProductGroup> CurtainComponentProductGroups { get; set; }

        public virtual DbSet<CommissionRate> CommissionRates { get; set; }

        public virtual DbSet<DeliveryMethod> DeliveryMethods { get; set; }


        #region DisCounts
        public virtual DbSet<Discount> Discounts { get; set; }
        #endregion

        #region Permissions
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        #endregion

        #region Properties
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<ProductProperty> ProductProperties { get; set; }
        public virtual DbSet<PropertyTechnicalProduct> PropertyTechnicalProducts { get; set; }
        #endregion

        public virtual DbSet<SlideShow> SlideShows { get; set; }

        #region Settings
        public virtual DbSet<Setting> Setting { get; set; }
        #endregion
        public virtual DbSet<PropertyTitle> PropertyTitles { get; set; }
        public virtual DbSet<PropertyTechnical> propertyTechnicals { get; set; }

        public virtual DbSet<Sms> SentMessages { get; set; }


        public virtual DbSet<AdviceModel> Advices { get; set; }

        public virtual DbSet<TransactionModel> Transactions { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            modelBuilder.Entity<ProductGroup>()
                .HasQueryFilter(u => !u.IsDelete);

            modelBuilder.Entity<Product>()
                .HasQueryFilter(u => !u.IsDelete);

            modelBuilder.Entity<ProductGallery>()
                .HasQueryFilter(u => !u.isDelete);



            base.OnModelCreating(modelBuilder);
        }
    }
}
