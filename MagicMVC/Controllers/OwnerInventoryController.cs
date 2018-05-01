using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MagicMVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MagicMVC.Controllers
{
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

    }

}
