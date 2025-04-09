using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CreditApplication.Data;
using CreditApplication.Models;

namespace CreditApplication.Pages.ClientFinancials
{
    public class DeleteModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public DeleteModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ClientFinancial ClientFinancial { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientfinancial = await _context.ClientFinancials.FirstOrDefaultAsync(m => m.ID == id);

            if (clientfinancial == null)
            {
                return NotFound();
            }
            else
            {
                ClientFinancial = clientfinancial;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientfinancial = await _context.ClientFinancials.FindAsync(id);
            if (clientfinancial != null)
            {
                ClientFinancial = clientfinancial;
                _context.ClientFinancials.Remove(ClientFinancial);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
