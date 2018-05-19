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
        public String Name { get; set; }
        public String Email { get; set; }
        public String ID { get; set; }


        public Customer(MagicMVCContext context)
        {
            _context = context;
        }

        public async Task GetOrderHistory()
        {
            //var query = _context.OwnerInventory.Include(x => x.Product).Select(x => x);
            //query = query.OrderBy(x => x.Product.Name);
            return;
        }

        public async Task SaveToCart(Purchase p)
        {
            p.DateOfPurchase = DateTime.Now;
            _context.Update(p);
            await _context.SaveChangesAsync();

        }

        public async Task ProcessCart()
        {
            var query = _context.Purchases
                                    .Where(x => x.CustomerID == this.ID)
                                    .Where(x => x.Confirmed == false)
                                    .Include(x => x.Store)
                                        .ThenInclude(store => store.StoreInventoryList)
                                    .Include(x => x.Product);

            List<Purchase> purchaseList = await query.ToListAsync();

            foreach (Purchase purchase in purchaseList)
            {
                var store = purchase.Store;
                var storeInventory = purchase.Store.GetStoreInventory(purchase.ProductID);

                if (storeInventory.StockLevel < purchase.QuantityToPurchase)
                {
                    throw new Exception("Sorry.  We don't have enough of the following product to fill your order: " + purchase.Product.Name);
                }
                else
                {
                    storeInventory.StockLevel -= purchase.QuantityToPurchase;
                    purchase.Confirmed = true;
                    purchase.DateOfPurchase = DateTime.Now;
                    _context.Update(purchase);
                    _context.Update(storeInventory);
                    await _context.SaveChangesAsync();
                    //Confirm purchase on screen
                }
            }
        }
    }
}
