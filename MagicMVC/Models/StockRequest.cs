using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MagicMVC.Models
{
    public class StockRequest
    {


        public int StockRequestID { get; set; }
        [Display(Name = "Store ID")]
        public int StoreID { get; set; }
        [Display(Name = "Product ID")]
        public int ProductID { get; set; }
        [Range(1, Int32.MaxValue, ErrorMessage = "Quantity must be a number greater than zero.")]
        public int Quantity { get; set; }

        public Store Store { get; set; }
        public StoreInventory StoreInventory { get; set; }
        public Product Product { get; set; }
    }
}

