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

namespace CreditApplication.Pages.RepaymentPlans
{
    public class EditModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public EditModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RepaymentPlan RepaymentPlan { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repaymentplan =  await _context.RepaymentPlans.FirstOrDefaultAsync(m => m.ID == id);
            if (repaymentplan == null)
            {
                return NotFound();
            }
            RepaymentPlan = repaymentplan;
           ViewData["CreditID"] = new SelectList(_context.Credits, "ID", "ID");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(RepaymentPlan);
            var entry = _context.Entry(RepaymentPlan);
            entry.State = EntityState.Modified;
            entry.Property(c => c.CreatedOn).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RepaymentPlanExists(RepaymentPlan.ID))
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

        private bool RepaymentPlanExists(int id)
        {
            return _context.RepaymentPlans.Any(e => e.ID == id);
        }
    }
}
