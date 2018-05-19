﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MagicMVC.Models;
using MagicMVC.Data;

namespace MagicMVC.Controllers
{
    
    public class CustomerController : Controller
    {
        private readonly MagicMVCContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        Customer customer;
        Franchisee franchisee;
        String userID;


        public CustomerController(MagicMVCContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            customer = new Customer(_context);
            franchisee = new Franchisee(_context);
        }

        //protected override void Initialize(System.Web.Routing.RequestContext rContext, )
        //{

        //}


        // GET: Customer
        public async Task<IActionResult> Index(int id = 1)
        {
            //user = await _userManager.GetUserAsync(User);
            var productQuery = _context.StoreInventory.Include(s => s.Product).Include(s => s.Store).Where(p => p.StoreID == id);
            return View(await productQuery.ToListAsync());
        }

        // GET: Customer/Purchase/5
        public async Task<IActionResult> Purchase(int ProductID, int id = 1)
        {
            userID = _userManager.GetUserId(User);
            var storeInventory = await _context.StoreInventory
                                    .Where(r => r.ProductID == ProductID)
                                     .SingleOrDefaultAsync(m => m.StoreID == id);

            if (storeInventory == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID", storeInventory.ProductID);
            ViewData["StoreID"] = new SelectList(_context.Stores, "StoreID", "StoreID", storeInventory.StoreID);

            Purchase p = new Purchase { StoreID = id, ProductID = ProductID, QuantityToPurchase = 0, Confirmed = false, DateOfPurchase = DateTime.MinValue, CustomerID = userID}; 

            return View(p);
        }

        // POST: Customer/Purchase/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Purchase(int id, [Bind("StoreID,ProductID,QuantityToPurchase,Confirmed,DateOfPurchase,CustomerID")] Purchase p)
        {
            p.CustomerID = _userManager.GetUserId(User);

            if (id != p.StoreID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
 
                    franchisee.storeID = id;
                    await franchisee.ProcessSale(p);
                }
                catch (DbUpdateConcurrencyException)
                {
                    bool inventoryExists = await franchisee.StoreInventoryExists(p.ProductID);
                    if (!inventoryExists)
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
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID", p.ProductID);
            ViewData["StoreID"] = new SelectList(_context.Stores, "StoreID", "StoreID", p.StoreID);
            return View(p);
        }
    }
}
