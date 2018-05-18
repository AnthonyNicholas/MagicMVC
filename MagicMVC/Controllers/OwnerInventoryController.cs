using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MagicMVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using MagicMVC.Data;

namespace MagicMVC.Controllers
{
    [Authorize(Roles = Constants.OwnerRole)]
    public class OwnerInventoryController : Controller
    {
        private readonly MagicMVCContext _context;
        private Owner owner;

        public OwnerInventoryController(MagicMVCContext context)
        {
            _context = context;
            owner = new Owner(_context);

        }

        // Auto-parsed variables coming in from the request - there is a form on the page to send this data.        
        public async Task<IActionResult> Index(string productName)
        {
            var inventory = await this.owner.GetOwnerInventory();

            if (!string.IsNullOrWhiteSpace(productName))
            { 
                // Get single product matching the name given.
                inventory = await this.owner.GetOwnerInventory(productName);

                // Storing the search into ViewBag to populate the textbox with the same value for convenience.
                ViewBag.ProductName = productName;
            }

            // Passing a List<OwnerInventory> model object to the View.
            return View(inventory);
        }

        public IActionResult Error()

        {

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        }

        // GET: Store/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ownerInventory = await _context.OwnerInventory.Include(s => s.Product).SingleOrDefaultAsync(m => m.ProductID == id);
            if (ownerInventory == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID", ownerInventory.ProductID);
            ViewData["Name"] = ownerInventory.Product.Name;
            return View(ownerInventory);
        }

        // POST: Store/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,StockLevel")] OwnerInventory ownerInventory)
        {
            _context.Update(ownerInventory);
            await _context.SaveChangesAsync();
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID", ownerInventory.ProductID);
            return RedirectToAction(nameof(Index));
        }

        private bool OwnerInventoryExists(int id)
        {
            return _context.OwnerInventory.Any(e => e.ProductID == id);
        }



    }

}
