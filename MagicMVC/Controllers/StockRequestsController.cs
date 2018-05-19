using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MagicMVC.Models;
using MagicMVC.Data;

/*
 *StockRequestsController is used by the owner and the franchisees.  The franchisees can view stock requests for 
 * their store and can make new ones. The owner is able to view all the stock requests and to process them.
 */

namespace MagicMVC.Controllers
{
    [Authorize(Roles = Constants.OwnerRole)]
    public class StockRequestsController : Controller
    {
        private readonly MagicMVCContext _context;
        private Owner owner;

        public StockRequestsController(MagicMVCContext context)
        {
            _context = context;
            owner = new Owner(_context);

        }

        // GET: StockRequests
        public async Task<IActionResult> Index()
        {
            var query =
                from r in _context.StockRequests
                join o in _context.OwnerInventory on r.ProductID equals o.ProductID
                join p in _context.Products on r.ProductID equals p.ProductID
                join s in _context.Stores on r.StoreID equals s.StoreID
                select new StockRequestOwnerViewModel {
                    StockRequestID = r.StockRequestID,
                    StoreName = s.Name,
                    ProductName = p.Name,
                    Quantity = r.Quantity,
                    StockLevel = o.StockLevel,
                    Availability = (r.Quantity <= o.StockLevel)
                };

            return View(await query.ToListAsync());
        }

        // GET: StockRequests/Create
        public IActionResult Create()
        {
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID");
            ViewData["StoreID"] = new SelectList(_context.Stores, "StoreID", "StoreID");
            return View();
        }

        // POST: StockRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StockRequestID,StoreID,ProductID,Quantity")] StockRequest stockRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stockRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID", stockRequest.ProductID);
            ViewData["StoreID"] = new SelectList(_context.Stores, "StoreID", "StoreID", stockRequest.StoreID);
            return View(stockRequest);
        }

        // GET: StockRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockRequest = await _context.StockRequests
                .Include(s => s.Product)
                .Include(s => s.Store)
                .SingleOrDefaultAsync(m => m.StockRequestID == id);
            if (stockRequest == null)
            {
                return NotFound();
            }

            return View(stockRequest);
        }

        // POST: StockRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            StockRequest s = await _context.StockRequests.SingleOrDefaultAsync(m => m.StockRequestID == id);

            bool available = await owner.IsStockAvailable(s);

            if (available)
            {
                //_context.StockRequests.Remove(s);
                await owner.PerformStockRequest(s);
            }
            //_context.StockRequests.Remove(stockRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = Constants.OwnerRole)]
        private bool StockRequestExists(int id)
        {
            return _context.StockRequests.Any(e => e.StockRequestID == id);
        }
    }
}
