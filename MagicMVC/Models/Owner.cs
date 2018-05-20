using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MagicMVC.Models
{
    // Owner class
    // Note - this is not one of the classes being managed by Entity Framework.  
    // Rather reflects object oriented design & logic sitting with model rather than in
    // controller.

    public class Owner
    {
        private readonly MagicMVCContext _context;

        public Owner(MagicMVCContext context)
        {
            _context = context;
        }

        public async Task<List<OwnerInventory>> GetOwnerInventory()
        { 
            var query = _context.OwnerInventory.Include(x => x.Product).Select(x => x);
            query = query.OrderBy(x => x.Product.Name);
            return (await query.ToListAsync());
        }

        public async Task<List<OwnerInventory>> GetOwnerInventory(int productID)
        {
            var test = _context.OwnerInventory.First(); 

            var query = _context.OwnerInventory                 
                            .Include(x => x.Product)
                            .Where(x => x.ProductID == productID); 
            var list = await query.ToListAsync();

            return list;
        }

        public async Task<List<OwnerInventory>> GetOwnerInventory(string productName)
        {
            var query = _context.OwnerInventory
                            .Include(x => x.Product)
                            .Where(x => x.Product.Name.Contains(productName));
            return await query.ToListAsync();
        }


        public async Task<bool> IsStockAvailable(StockRequest s)
        {
            var query = await GetOwnerInventory(s.ProductID);  
            OwnerInventory item = query.First();
            var test = item.StockLevel;
            return item.StockLevel >= s.Quantity;
        }

        public async Task PerformStockRequest(StockRequest s)
        {
            var ownerItem = await _context.OwnerInventory.
                                    SingleOrDefaultAsync(r => r.ProductID == s.ProductID);

            var storeItem = await _context.StoreInventory.
                                    Where(r => r.StoreID == s.StoreID).
                                    SingleOrDefaultAsync(r => r.ProductID == s.ProductID);

            //Add product to store if not currently in Inventory.
            if (storeItem == null)
            {

                StoreInventory item = new StoreInventory {StoreID = s.StoreID, ProductID = s.ProductID, StockLevel = s.Quantity };
                _context.StoreInventory.Add(item);
                await _context.SaveChangesAsync();
            }
            else
            {
                storeItem.StockLevel += s.Quantity;
            }

            //Decrease owner product stock level.
            ownerItem.StockLevel -= s.Quantity;

            // Remove stock request
            _context.StockRequests.Remove(s);

            await _context.SaveChangesAsync();
        }
    }
}
