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
        private int storeID;

        public Franchisee(MagicMVCContext context, int storeID)
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

        public async Task AddItemToStoreInventory(StockRequest s)
        {
            //Add item to storeInventory
            StoreInventory item = new StoreInventory { StoreID = this.storeID, ProductID = s.ProductID, StockLevel = s.Quantity };
            this._context.StoreInventory.Add(item);
            this._context.SaveChanges();
            return;
        }


        //public async void MakeStockRequest(StockRequest s)
        //{
        //    return;
        //}
    }
}
