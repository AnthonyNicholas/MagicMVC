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
using MagicMVC.Utilities;

namespace MagicMVC.Controllers
{
    [Authorize(Roles=Constants.CustomerRole)]
    public class ShoppingCartController : Controller
    {
        private readonly MagicMVCContext _context;

        public ShoppingCartController(MagicMVCContext context)
        {
            _context = context;
        }

        // GET: ShoppingCart
        public async Task<IActionResult> Index()
        {
            return View(await _context.Purchases.ToListAsync());
        }


        // GET: ShoppingCart/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchases.SingleOrDefaultAsync(m => m.PurchaseID == id);
            if (purchase == null)
            {
                return NotFound();
            }
            return View(purchase);
        }

        // POST: ShoppingCart/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PurchaseID,StoreID,ProductID,QuantityToPurchase,CustomerID,Confirmed,DateOfPurchase")] Purchase purchase)
        {
            if (id != purchase.PurchaseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseExists(purchase.PurchaseID))
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
            return View(purchase);
        }

        // GET: ShoppingCart/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchases
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
        public ActionResult Pay()
        {
            return View(new CreditCardForm { CreditCardType = CardType.MasterCard });
        }

        // POST: PaymentPage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Pay(CreditCardForm creditCardForm)
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

            return View("PaymentReceived");
        }

    }
}
