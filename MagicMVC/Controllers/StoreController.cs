using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MagicMVC.Models;
using MagicMVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MagicMVC.Controllers
{

    // StoreController: Franchisee needs to see stock & be able to make stock requests
    [Authorize(Roles=Constants.FranchiseeRole)]
    public class StoreController : Controller
    {
        public const string SessionFranchiseStoreID = "_FranchiseStoreID";
        private readonly MagicMVCContext _context;
        private Store Store;
        private string UserID;

        public StoreController(MagicMVCContext context)
        {
            _context = context;
            
        }

        private void FetchStore()
        {
            var sessionID = HttpContext.Session.GetInt32(SessionFranchiseStoreID);
            var claimsIdentity = User.Identity as ClaimsIdentity;
            UserID = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            Store = _context.Stores.FirstOrDefault(s => s.FranchiseeUser == UserID);
        }

        // GET: Store Index - shows inventory for the user's store.

        public async Task<IActionResult> Unstocked()
        {
            FetchStore();
            int storeID = Store.StoreID;
            Franchisee franchisee = new Franchisee(_context, UserID, storeID);

            // Load products in OwnerInventory that aren't in this Store's Inventory
            var storeInventory = await franchisee.GetStoreInventory();
            List<OwnerInventory> inventory = await _context.OwnerInventory.Include(i => i.Product).Where(o => !storeInventory.Any(s => s.ProductID == o.ProductID)).ToListAsync();

            //Passing a List<OwnerInventory> model object to the View.

            return View(inventory);
        }

        public async Task<IActionResult> Index(string productName)
        {
            FetchStore();
            int storeID = Store.StoreID;

            Franchisee franchisee = new Franchisee(_context, UserID, storeID);
            
            List<StoreInventory> inventory = await franchisee.GetStoreInventory();
            
            // Eager loading the Product table - join between StoreInventory and the Product table.
            //var storeQuery = await _context.Stores.Where(s => s.StoreID == id).ToListAsync();
            //var store = storeQuery.First();

            var productQuery = _context.StoreInventory.Include(x => x.Product).Where(p => p.StoreID == storeID);

            if (!string.IsNullOrWhiteSpace(productName))
            {
                inventory = await franchisee.GetStoreInventory(productName);
                
                // Storing the search into ViewBag to populate the textbox with the same value for convenience.
                ViewBag.ProductName = productName;
            }


            //Passing a List<OwnerInventory> model object to the View.

            return View(inventory);
        }

        // GET: Store/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeInventory = await _context.StoreInventory
                .Include(s => s.Product)
                .Include(s => s.Store)
                .SingleOrDefaultAsync(m => m.StoreID == id);
            if (storeInventory == null)
            {
                return NotFound();
            }

            return View(storeInventory);
        }

        // GET: Store/MakeStockRequest
        public IActionResult MakeStockRequest(int? id)
        {
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID", id ?? _context.Products.First().ProductID);
            return View();
        }

        // POST: Store/MakeStockRequest
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeStockRequest([Bind("StoreID,ProductID,Quantity")] StockRequest stockRequest)
        {
            if (ModelState.IsValid)
            {
                FetchStore();
                stockRequest.StoreID = Store.StoreID;
                _context.Add(stockRequest);

                // if this product wasn't previously in stock, add it to the store stock with quantity 1
                if (!Store.StoreInventoryList.Any(si => si.ProductID == stockRequest.ProductID)) {
                    _context.Add(new StoreInventory
                    {
                        StoreID = Store.StoreID,
                        Product = _context.Products.Find(stockRequest.ProductID),
                        StockLevel = 1
                    });
                }
                

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stockRequest);
        }

        // GET: Store/Create
        public IActionResult Create()
        {
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID");
            ViewData["StoreID"] = new SelectList(_context.Stores, "StoreID", "StoreID");
            return View();
        }

        // POST: Store/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StoreID,ProductID,StockLevel")] StoreInventory storeInventory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(storeInventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID", storeInventory.ProductID);
            ViewData["StoreID"] = new SelectList(_context.Stores, "StoreID", "StoreID", storeInventory.StoreID);
            return View(storeInventory);
        }

        // GET: Store/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeInventory = await _context.StoreInventory.SingleOrDefaultAsync(m => m.StoreID == id);
            if (storeInventory == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID", storeInventory.ProductID);
            ViewData["StoreID"] = new SelectList(_context.Stores, "StoreID", "StoreID", storeInventory.StoreID);
            return View(storeInventory);
        }

        // POST: Store/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StoreID,ProductID,StockLevel")] StoreInventory storeInventory)
        {
            if (id != storeInventory.StoreID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(storeInventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreInventoryExists(storeInventory.StoreID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID", storeInventory.ProductID);
            ViewData["StoreID"] = new SelectList(_context.Stores, "StoreID", "StoreID", storeInventory.StoreID);
            return View(storeInventory);
        }

        // GET: Store/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeInventory = await _context.StoreInventory
                .Include(s => s.Product)
                .Include(s => s.Store)
                .SingleOrDefaultAsync(m => m.StoreID == id);
            if (storeInventory == null)
            {
                return NotFound();
            }

            return View(storeInventory);
        }

        // POST: Store/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var storeInventory = await _context.StoreInventory.SingleOrDefaultAsync(m => m.StoreID == id);
            _context.StoreInventory.Remove(storeInventory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoreInventoryExists(int id)
        {
            return _context.StoreInventory.Any(e => e.StoreID == id);
        }
    }
}
