using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CreditApplication.Data;
using CreditApplication.Models;

namespace CreditApplication.Pages.Credits
{
    public class DeleteModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public DeleteModel(CreditApplication.Data.CreditApplicationDbContext context)
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

            var credit = await _context.Credits.FirstOrDefaultAsync(m => m.ID == id);

            if (credit == null)
            {
                return NotFound();
            }
            else
            {
                Credit = credit;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var credit = await _context.Credits.FindAsync(id);
            if (credit != null)
            {
                Credit = credit;
                _context.Credits.Remove(Credit);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
