
using System.ComponentModel.DataAnnotations;
using MadWin.Core.Entities.Products;

namespace MadWin.Core.Entities.Properties
{
    public class PropertyTechnicalProduct
    {
        #region ctor
        public PropertyTechnicalProduct()
        {

        }
        #endregion
        #region Field
        public int propertyTechnicalProductId { get; set; }
        public int productId { get; set; }
        public int propertyTechnicalId { get; set; }

        [Display(Name = "مقدار")]
        public string propertyTechnicalProductValue { get; set; }
        #endregion
        #region Relationship
        public Product Product { get; set; }
        public PropertyTechnical PropertyTechnical { get; set; }
        #endregion
    }
}
