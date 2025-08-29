using System;
using System.Collections.Generic;
using System.Text;

namespace MadWin.Core.DTOs.Orders
{
    /// <summary>
    /// Represents a best sellers report line
    /// </summary>
    public class BestsellersViewModel
    {
        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public int productId { get; set; }

        /// <summary>
        /// Gets or sets the total amount
        /// </summary>
        public decimal totalAmount { get; set; }

        /// <summary>
        /// Gets or sets the total quantity
        /// </summary>
        public int totalQuantity { get; set; }
    }
}
