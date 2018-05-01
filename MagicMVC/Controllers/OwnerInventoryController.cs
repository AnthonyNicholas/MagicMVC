using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MagicMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicMVC.Controllers
{
    public class OwnerInventoryController : Controller
    {
        private readonly MagicMVCContext _context;

        public OwnerInventoryController(MagicMVCContext context)
        {
            _context = context;
        }


        // Auto-parsed variables coming in from the request - there is a form on the page to send this data.

        public async Task<IActionResult> Index(string productName)

        {

            // Eager loading the Product table - join between OwnerInventory and the Product table.

            var query = _context.OwnerInventory.Include(x => x.Product).Select(x => x);


            if (!string.IsNullOrWhiteSpace(productName))

            {

                // Adding a where to the query to filter the data.

                // Note for the first request productName is null thus the where is not always added.

                query = query.Where(x => x.Product.Name.Contains(productName));


                // Storing the search into ViewBag to populate the textbox with the same value for convenience.

                ViewBag.ProductName = productName;

            }


            // Adding an order by to the query for the Product name.

            query = query.OrderBy(x => x.Product.Name);


            // Passing a List<OwnerInventory> model object to the View.

            return View(await query.ToListAsync());

        }

        public IActionResult Error()

        {

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        }

    }

}
