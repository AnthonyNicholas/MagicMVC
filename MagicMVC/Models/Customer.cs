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
        public String Name { get; set;  }
        public String Email { get; set; }

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

        public async Task ProcessSale(Purchase p)
        {
            var query = _context.StoreInventory
                                    .Include(x => x.Product)
                                    .Where(x => x.StoreID == p.StoreID)
                                    .Where(x => x.ProductID == p.ProductID);

            StoreInventory item = (await query.ToListAsync()).First();
                   
            if (item.StockLevel < p.QuantityToPurchase)
            {
                throw new Exception("Insufficient Stock to make that purchase");
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




    }
}
