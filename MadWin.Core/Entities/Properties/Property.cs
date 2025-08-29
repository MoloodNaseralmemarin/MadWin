using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MadWin.Core.Entities.Properties
{
    public class Property
    {
        #region Ctor
        public Property()
        {

        }
        #endregion
        #region Field
        [Key]
        public int propertyId { get; set; }
        public string Title { get; set; }
        #endregion
        #region Relationship
        public PropertyTitle PropertyTitle { get; set; }
        public List<ProductProperty> ProductProperties { get; set; }
        #endregion
    }
}
