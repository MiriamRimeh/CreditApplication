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

namespace CreditApplication.Pages.FinancialOperations
{
    public class EditModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public EditModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public FinancialOperation FinancialOperation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var financialoperation =  await _context.FinancialOperations.FirstOrDefaultAsync(m => m.ID == id);
            if (financialoperation == null)
            {
                return NotFound();
            }
            FinancialOperation = financialoperation;
           ViewData["CreditID"] = new SelectList(_context.Credits, "ID", "ID");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            _context.Attach(FinancialOperation);
            var entry = _context.Entry(FinancialOperation);
            entry.State = EntityState.Modified;
            entry.Property(c => c.CreatedOn).IsModified = false;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FinancialOperationExists(FinancialOperation.ID))
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

        private bool FinancialOperationExists(int id)
        {
            return _context.FinancialOperations.Any(e => e.ID == id);
        }
    }
}
