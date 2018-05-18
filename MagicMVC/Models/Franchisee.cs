using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MagicMVC.Models
{
    // Frnachisee class
    // Note - this is not one of the classes being managed by Entity Framework.  
    // Rather reflects object oriented design & logic sitting with model rather than in
    // controller.

    public class Franchisee
    {
        private readonly MagicMVCContext _context;
        [Key]
        public string UserID { get; set; }
        public int StoreID { get; set; }

        public Franchisee(MagicMVCContext context, string userID = "", int storeID = 1)
        {
            _context = context;
            this.UserID = userID;
            this.StoreID = storeID;
        }

        public async Task<Store> GetStore()
        {
            var query = _context.Stores.Where(x => x.StoreID == this.StoreID);
            Store store = (await query.ToListAsync()).First();
            return store;
        }

        public async Task<List<StoreInventory>> GetStoreInventory()
        {
            var query = _context.StoreInventory.Include(x => x.Product).Where(x => x.StoreID == this.StoreID);
            query = query.OrderBy(x => x.Product.Name);
            return (await query.ToListAsync());
        }

        public async Task<List<StoreInventory>> GetStoreInventory(int productID)
        {
            var query = _context.StoreInventory
                            .Include(x => x.Product)
                            .Where(x => x.StoreID == this.StoreID)
                            .Where(x => x.ProductID == productID);

            return await query.ToListAsync();
        }

        public async Task<List<StoreInventory>> GetStoreInventory(string productName)
        {
            var query = _context.StoreInventory
                            .Include(x => x.Product)
                            .Where(x => x.StoreID == this.StoreID)
                            .Where(x => x.Product.Name.Contains(productName));

            return await query.ToListAsync();
        }

        public async Task<bool> StoreInventoryExists(int productID)
        {
            return _context.StoreInventory
                            .Where(x => x.StoreID == this.StoreID)
                            .Any(e => e.ProductID == productID);
        }

        public async Task AddItemToStoreInventory(StockRequest s)
        {
            //Add item to storeInventory
            StoreInventory item = new StoreInventory { StoreID = this.StoreID, ProductID = s.ProductID, StockLevel = s.Quantity };
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
                _context.Update(item);
                await _context.SaveChangesAsync();
                //Confirm purchase on screen
            }
        }



        //public async void MakeStockRequest(StockRequest s)
        //{
        //    return;
        //}
    }
}
