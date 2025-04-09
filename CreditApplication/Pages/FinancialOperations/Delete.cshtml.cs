using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CreditApplication.Data;
using CreditApplication.Models;

namespace CreditApplication.Pages.FinancialOperations
{
    public class DeleteModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public DeleteModel(CreditApplication.Data.CreditApplicationDbContext context)
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

            var financialoperation = await _context.FinancialOperations.FirstOrDefaultAsync(m => m.ID == id);

            if (financialoperation == null)
            {
                return NotFound();
            }
            else
            {
                FinancialOperation = financialoperation;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var financialoperation = await _context.FinancialOperations.FindAsync(id);
            if (financialoperation != null)
            {
                FinancialOperation = financialoperation;
                _context.FinancialOperations.Remove(FinancialOperation);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
