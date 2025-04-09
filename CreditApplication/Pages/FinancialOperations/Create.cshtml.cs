using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CreditApplication.Data;
using CreditApplication.Models;

namespace CreditApplication.Pages.FinancialOperations
{
    public class CreateModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public CreateModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CreditID"] = new SelectList(_context.Credits, "ID", "ID");
            return Page();
        }

        [BindProperty]
        public FinancialOperation FinancialOperation { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.FinancialOperations.Add(FinancialOperation);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
