using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MadWin.Core.Entities.Common;
using MadWin.Core.Entities.Products;

namespace MadWin.Core.Entities.Factors
{
    [Table("FactorDetails", Schema = "Factors")]
    public class FactorDetail:BaseEntity
    {
        public FactorDetail()
        {  
        }

        [Required]
        public int FactorId { get; set; }

        [Required]
        public int ProductId { get; set; }

        /// <summary>
        /// تعداد
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// آخرین قیمت                                                                                                                                                                                                                                      
        /// </summary>
        [Required]
        public decimal Price { get; set; }


        #region Relationship
        public Factor Factor { get; set; }
        public Product Product { get; set; }
        #endregion
    }
}
