using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace MagicMVC.Models
{
    public class Purchase
    {
        public int StoreID { get; set; }
        public int ProductID { get; set; }
        public int QuantityToPurchase { get; set; }
    }
}

