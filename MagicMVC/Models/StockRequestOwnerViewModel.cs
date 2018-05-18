using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace MagicMVC.Models
{
    public class StockRequestOwnerViewModel
    {
        [Display(Name = "Request ID")]
        public int StockRequestID { get; set; }

        [Display(Name = "Store")]
        public string StoreName { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

            
        public int Quantity { get; set; }

        [Display(Name = "Stock Level")]
        public int StockLevel { get; set; }

        [Display(Name = "Stock Available?")]
        public bool Availability { get; set; }
    }
}

