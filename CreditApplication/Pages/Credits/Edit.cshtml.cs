using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CreditApplication.Data;
using CreditApplication.Models;

namespace CreditApplication.Pages.Credits
{
    public class EditModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public EditModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Credit Credit { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var credit =  await _context.Credits.FirstOrDefaultAsync(m => m.ID == id);

            if (credit == null)
            {
                return NotFound();
            }
            Credit = credit;
           //ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "EGN");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

           //ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "EGN");

            if (Credit.CreditAmount < 300 || Credit.CreditAmount > 5000)
            {
                ModelState.AddModelError(
                    "Credit.CreditAmount",
                    "Сумата на кредита трябва да бъде между 300 лв и 5000 лв."
                );
                return Page();
            }
            if (Credit.CreditPeriod < 5 || Credit.CreditPeriod > 24)
            {
                ModelState.AddModelError(
                    "Credit.CreditPeriod",
                    "Периодът на кредита трябва да е между 5 и 24 месеца."
                );
                return Page();
            }

            var originalCredit = await _context.Credits.FindAsync(Credit.ID);
            if (originalCredit == null)
            {
                return NotFound();
            }

            originalCredit.CreditAmount = Credit.CreditAmount;
            originalCredit.CreditPeriod = Credit.CreditPeriod;
            originalCredit.TotalCreditAmount = Credit.TotalCreditAmount;
            originalCredit.MonthlyInstallment = Credit.MonthlyInstallment;

            //_context.Attach(Credit);
            //var entry = _context.Entry(Credit);
            //entry.State = EntityState.Modified;
            //entry.Property(c => c.CreatedOn).IsModified = false;
            //entry.Property(c => c.Status).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CreditExists(Credit.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }


            return RedirectToPage("./Index");
        }

        private bool CreditExists(int id)
        {
            return _context.Credits.Any(e => e.ID == id);
        }
    }
}
