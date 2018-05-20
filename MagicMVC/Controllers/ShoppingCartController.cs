using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MagicMVC.Models;
using MagicMVC.Data;
using Microsoft.AspNetCore.Authorization;
using MagicMVC.Utilities;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MagicMVC.Controllers
{
    [Authorize(Roles=Constants.CustomerRole)]
    public class ShoppingCartController : Controller
    {
        private readonly MagicMVCContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        Customer customer;

        public ShoppingCartController(MagicMVCContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            customer = new Customer(_context);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //Set the customer.ID after class constructed   
            customer.ID = _userManager.GetUserId(User);
        }

        // GET: ShoppingCart
        public async Task<IActionResult> Index()
        {
            var purchaseList = await customer.GetCartItems();
            ViewData["TotalPrice"] = decimal.Round(purchaseList.Sum(p => p.SubTotal), 2);
            return View(purchaseList);
        }

        // GET: ShoppingCart/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchases
                            .Include(p => p.Product)
                            .Include(p => p.Store)
                            .SingleOrDefaultAsync(m => m.PurchaseID == id);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // POST: ShoppingCart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchase = await _context.Purchases.SingleOrDefaultAsync(m => m.PurchaseID == id);
            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseExists(int id)
        {
            return _context.Purchases.Any(e => e.PurchaseID == id);
        }

        // GET: PaymentPage
        public async Task<IActionResult> Pay()
        {
            var purchaseList = await customer.GetCartItems();
            ViewData["TotalPrice"] = decimal.Round(purchaseList.Sum(p => p.SubTotal), 2);
            return View(new CreditCardForm { CreditCardType = CardType.MasterCard });
        }

        // POST: PaymentPage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay(CreditCardForm creditCardForm)
        {
            // Validate card type.
            CardType expectedCardType = CardTypeInfo.GetCardType(creditCardForm.CreditCardNumber);
            if (expectedCardType == CardType.Unknown || expectedCardType != creditCardForm.CreditCardType)
            {
                ModelState.AddModelError("CreditCardType", "The Credit Card Type field does not match against the credit card number.");
            }

            if (!ModelState.IsValid)
            {
                return View(creditCardForm);
            }

            var purchaseList = await customer.ProcessCart();

            Order order = new Order();
            order.CustomerID = customer.ID;
            order.Date = DateTime.Now;
            order.TotalPrice = decimal.Round(purchaseList.Sum(p => p.SubTotal), 2);
            _context.Update(order);
            await _context.SaveChangesAsync();

            ViewData["orderID"] = order.OrderID;
            ViewData["TotalPrice"] = order.TotalPrice;
            ViewData["Date"] = order.Date;

            return View("Confirmation", purchaseList);

        }

    }
}
