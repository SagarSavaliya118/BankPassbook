using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BankPassbook.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Data;
using BankPassbook.Attributes;

namespace BankPassbook.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly BankPassbookDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TransactionController(BankPassbookDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            return _context.Transactions != null ? 
                          View(await _context.Transactions.Where(entry => entry.User.Id == userId).ToListAsync()) :
                          Problem("Entity set 'BankPassbookDbContext.Transactions'  is null.");
        }

        // GET: Transaction/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Transaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountNumber,BeneficiaryName,BankName,SwiftCode,Amount,Date")] Transaction transaction)
        {
            var user = _userManager.GetUserAsync(User);
            transaction.User = user.Result;
            _context.Add(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Transaction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return View(transaction);
        }

        // POST: Transaction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountNumber,BeneficiaryName,BankName,SwiftCode,Amount")] Transaction updatedTransaction)
        {
            if (id != updatedTransaction.Id)
            {
                return NotFound();
            }

            try
            {
                var transaction = await _context.Transactions.FindAsync(updatedTransaction.Id);
                transaction.AccountNumber = updatedTransaction.AccountNumber;
                transaction.BeneficiaryName = updatedTransaction.BeneficiaryName;
                transaction.BankName = updatedTransaction.BankName;
                transaction.SwiftCode = updatedTransaction.SwiftCode;
                transaction.Amount = updatedTransaction.Amount;

                _context.Update(transaction);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(updatedTransaction.Id))
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

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Transactions == null)
            {
                return Problem("Entity set 'BankPassbookDbContext.Transactions'  is null.");
            }
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
          return (_context.Transactions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
