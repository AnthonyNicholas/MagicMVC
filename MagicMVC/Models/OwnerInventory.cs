using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MagicMVC.Models
{
    public class OwnerInventory
    {
        [Key, ForeignKey("Product"), Display(Name = "Product ID")]
        public int ProductID { get; set; }
        public Product Product { get; set; }

        [Display(Name = "Stock Level")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Quantity must be a number equal to or greater than zero.")]
        public int StockLevel { get; set; }
    }

    public static class OwnerInventoryExtensions
    {
        public static OwnerInventory GetOwnerInventory(this ICollection<OwnerInventory> inventory, int productID)
        {
            return inventory.FirstOrDefault(x => x.ProductID == productID);
        }
    }



}
