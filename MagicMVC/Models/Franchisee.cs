using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;


namespace MagicMVC.Models
{
    // Franchisee class
    // Note - this is not one of the classes being managed by Entity Framework.  
    // This class contains methods for things the Franchisee needs to do - getting the inventory for their store,
    // processing sales and making stock requests.

    public class Franchisee
    {
        private readonly MagicMVCContext _context;
        public int storeID { get; set; }

        public Franchisee(MagicMVCContext context, int storeID = 1)
        {
            _context = context;
            this.storeID = storeID;
        }

        public async Task<Store> GetStore()
        {
            var query = _context.Stores.Where(x => x.StoreID == this.storeID);
            Store store = (await query.ToListAsync()).First();
            return store;
        }

        public async Task<List<StoreInventory>> GetStoreInventory()
        {
            var query = _context.StoreInventory.Include(x => x.Product).Where(x => x.StoreID == this.storeID);
            query = query.OrderBy(x => x.Product.Name);
            return (await query.ToListAsync());
        }

        public async Task<List<StoreInventory>> GetStoreInventory(int productID)
        {
            var query = _context.StoreInventory
                            .Include(x => x.Product)
                            .Where(x => x.StoreID == this.storeID)
                            .Where(x => x.ProductID == productID);

            return await query.ToListAsync();
        }

        public async Task<List<StoreInventory>> GetStoreInventory(string productName)
        {
            var query = _context.StoreInventory
                            .Include(x => x.Product)
                            .Where(x => x.StoreID == this.storeID)
                            .Where(x => x.Product.Name.Contains(productName));

            return await query.ToListAsync();
        }

        public async Task<List<StoreInventory>> GetStoreInventoryBelowThreshold(int threshold)
        {
            var query = _context.StoreInventory
                            .Include(x => x.Product)
                            .Where(x => x.StoreID == this.storeID)
                            .Where(x => x.IsBelowThreshold(threshold));

            return await query.ToListAsync();
        }

        public async Task<bool> StoreInventoryExists(int productID)
        {
            return await _context.StoreInventory
                            .Where(x => x.StoreID == this.storeID)
                            .AnyAsync(e => e.ProductID == productID);
        }

        public async Task AddItemToStoreInventory(StockRequest s)
        {
            //Add item to storeInventory
            StoreInventory item = new StoreInventory { StoreID = this.storeID, ProductID = s.ProductID, StockLevel = s.Quantity };
            this._context.StoreInventory.Add(item);
            this._context.SaveChanges();
            return;
        }

        public async Task ProcessSale(Purchase p)
        {
            StoreInventory item = (await GetStoreInventory(p.ProductID)).First();

            if (item.StockLevel < p.QuantityToPurchase)
            {
                throw new Exception("Insufficient Stock to make that purchase");
                //Send to error
                //Console.WriteLine($"{product.Name} has no stock available.");
            }
            else
            {
                item.StockLevel -= p.QuantityToPurchase;
                p.Confirmed = true;
                p.DateOfPurchase = DateTime.Now;
                _context.Update(item);
                _context.Update(p);
                await _context.SaveChangesAsync();
                //Confirm purchase on screen
            }
        }

        public async void MakeStockRequest(StockRequest s)
        {

            return;
        }
    }
}
