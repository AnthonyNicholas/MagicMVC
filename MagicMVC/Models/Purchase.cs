using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System;

namespace MagicMVC.Models
{
    public class Purchase
    {
        public int PurchaseID { get; set; }
        public int StoreID { get; set; }
        public int ProductID { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Quantity must be a number greater than zero.")]
        public int QuantityToPurchase { get; set; }

        public String CustomerID { get; set; }
        public Boolean Confirmed { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public Store Store { get; set; }
        public Product Product { get; set; }

        public decimal SubTotal {
            get
            {
                if (Product != null)
                {
                    return Product.Price * QuantityToPurchase;
                }
                else
                {
                    return 0;
                }
            }
        }

    }
}

