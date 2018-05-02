using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MagicMVC.Models
{
    // Customer class
    // Note - this is not one of the classes being managed by Entity Framework.  
    // Rather reflects object oriented design & logic sitting with model rather than in
    // controller.

    //Customer makes purchases, has a shopping cart and an order history

    public class Customer
    {
        private readonly MagicMVCContext _context;

        public Customer(MagicMVCContext context)
        {
            _context = context;
        }

        public async Task PurchaseProduct(StoreInventory item, int quantityToPurchase)
        {
            Franchisee f = new Franchisee(_context, item.StoreID);

            //var storeItem = (await f.GetStoreInventory(item.ProductID)).First();

            ////var query = _context.OwnerInventory.Include(x => x.Product).Select(x => x);
            ////query = query.OrderBy(x => x.Product.Name);
            if (item.StockLevel < quantityToPurchase)
            {
                //Send to error
                //Console.WriteLine($"{product.Name} has no stock available.");
            }
            else
            {
                item.StockLevel -= quantityToPurchase;
                await _context.SaveChangesAsync();
                //Confirm purchase on screen
            }
        }

        public async Task AddToShoppingCart()
        {
            //var query = _context.OwnerInventory.Include(x => x.Product).Select(x => x);
            //query = query.OrderBy(x => x.Product.Name);
            return;
        }

        public async Task GetOrderHistory()
        {
            //var query = _context.OwnerInventory.Include(x => x.Product).Select(x => x);
            //query = query.OrderBy(x => x.Product.Name);
            return;
        }



    }
}
