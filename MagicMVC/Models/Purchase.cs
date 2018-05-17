using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MagicMVC.Models
{
    public class Purchase
    {
        public int PurchaseID { get; set; }
        public int StoreID { get; set; }
        public int ProductID { get; set; }
        public int QuantityToPurchase { get; set; }
        public String CustomerID { get; set; }
        public Boolean Confirmed { get; set; }
        public DateTime DateOfPurchase { get; set; }

    }
}

