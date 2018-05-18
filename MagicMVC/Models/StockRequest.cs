using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace MagicMVC.Models
{
    public class StockRequest
    {

        public int StockRequestID { get; set; }
        public int StoreID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }

        public Store Store { get; set; }
        public StoreInventory StoreInventory { get; set; }
        public Product Product { get; set; }
    }
}

