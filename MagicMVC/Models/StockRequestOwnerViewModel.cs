using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace MagicMVC.Models
{
    public class StockRequestOwnerViewModel
    {
        public int StockRequestID { get; set; }
        public string StoreName { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int StockLevel { get; set; }
        public bool Availability { get; set; }
    }
}

