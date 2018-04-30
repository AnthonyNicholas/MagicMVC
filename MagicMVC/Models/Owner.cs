using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace MagicMVC.Models
{
    //Owner class - Is not one of the classes being managed by Entity Framework.  
    // THerefore we can access and use the context within in [I assume??]

    public class Owner
    {
        private readonly MagicMVCContext _context;
        public List<OwnerInventory> OwnerInventoryList { get; set; }
        public ICollection<StockRequest> StockRequestList { get; set; } = new List<StockRequest>();

        public Owner(MagicMVCContext context)
        {
            _context = context;
        }

        public OwnerInventory GetOwnerInventory(int productID)
        {
            return OwnerInventoryList.GetOwnerInventory(productID);
        }

        public bool IsStockAvailable(StockRequest s)
        {
            OwnerInventory item = GetOwnerInventory(s.ProductID);
            return item.StockLevel >= s.Quantity;
        }

        public void PerformStockRequest(StockRequest s)
        {
            if (!IsStockAvailable(s))
            {
                throw new InvalidOperationException();
            }

            //Decrease owner product stock level.
            OwnerInventory owner_item = GetOwnerInventory(s.ProductID);
            owner_item.StockLevel -= s.Quantity;

            StoreInventory store_item = s.Store.GetStoreInventory(s.ProductID);

            if (store_item == null)
            {
                //Add product to store.
                s.Store.StoreInventoryList.Add(new StoreInventory { StoreID = s.StoreID, ProductID = s.ProductID, StockLevel = s.Quantity });
            }
            else
            {
                //Increase store product stock level.
                store_item.StockLevel += s.Quantity;
            }

            //Remove stock request from system.
            this.StockRequestList.Remove(s);
          }
    }
}
