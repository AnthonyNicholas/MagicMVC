using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MagicMVC.Models
{
    public class Store
    {
        public int StoreID { get; set; }

        [Display(Name="Store Name")]
        public string Name { get; set; }
        public string FranchiseeUser { get; set; }
        public ICollection<StoreInventory> StoreInventoryList { get; } = new List<StoreInventory>();
        public ICollection<StockRequest> StockRequestList { get; set; } = new List<StockRequest>();

        public StoreInventory GetStoreInventory(int productID)
        {
            return StoreInventoryList.GetStoreInventory(productID);
        }
    }
}
