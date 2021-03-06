﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MagicMVC.Models;
using MagicMVC.Data;

namespace MagicMVC.Controllers
{
    [Authorize(Roles = Constants.FranchiseeRole)]
    public class Franchisee_StockRequestsController : Controller
    {
        private readonly MagicMVCContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private Franchisee franchisee;
        private ApplicationUser user;


        public Franchisee_StockRequestsController(MagicMVCContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            franchisee = new Franchisee(_context);
            _userManager = userManager;
        }


        // GET: Franchisee_StockRequests 
        // Pass List of type StockRequestOwnerViewModel - however limit the list to a single store
        public async Task<IActionResult> Index()
        {
            user = await _userManager.GetUserAsync(User);

            //Get the StoreID associated with this user - JUST USING 1 FOR STOREID FOR NOW
            var query =
                from r in _context.StockRequests
                join o in _context.OwnerInventory on r.ProductID equals o.ProductID
                join p in _context.Products on r.ProductID equals p.ProductID
                join s in _context.Stores on r.StoreID equals s.StoreID
                where r.StoreID == 1
                select new StockRequestOwnerViewModel
                {
                    StockRequestID = r.StockRequestID,
                    StoreName = s.Name,
                    ProductName = p.Name,
                    Quantity = r.Quantity,
                    StockLevel = o.StockLevel,
                    Availability = (r.Quantity <= o.StockLevel)
                };

            return View(await query.ToListAsync());
        }

        // GET: Franchisee_StockRequests/Create
        public IActionResult Create()
        {
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID");
            ViewData["StoreID"] = new SelectList(_context.Stores, "StoreID", "StoreID");
            ViewData["StoreID"] = new SelectList(_context.StoreInventory, "StoreID", "StoreID");
            return View();
        }

        // POST: Franchisee_StockRequests/Create
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
            ViewData["StoreID"] = new SelectList(_context.StoreInventory, "StoreID", "StoreID", stockRequest.StoreID);
            return View(stockRequest);
        }

        private bool StockRequestExists(int id)
        {
            return _context.StockRequests.Any(e => e.StockRequestID == id);
        }
    }
}
