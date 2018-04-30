using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace MagicMVC.Models
{
    public class StoreInventory
    {
        public int StoreID { get; set; }
        public int ProductID { get; set; }
        public int StockLevel { get; set; }

        public Product Product { get; set; }
        public Store Store { get; set; }
    
        public bool IsBelowThreshold(int threshold)
        {
             return StockLevel < threshold;
        }
    }

    public static class StoreInventoryExtensions
    {
        public static StoreInventory GetStoreInventory(this ICollection<StoreInventory> inventory, int productID)
        {
            return inventory.FirstOrDefault(x => x.ProductID == productID);
        }
    }

}
