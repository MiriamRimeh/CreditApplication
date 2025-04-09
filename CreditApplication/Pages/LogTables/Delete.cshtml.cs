using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CreditApplication.Data;
using CreditApplication.Models;

namespace CreditApplication.Pages.LogTables
{
    public class DeleteModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public DeleteModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LogTable LogTable { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logtable = await _context.LogTables.FirstOrDefaultAsync(m => m.ID == id);

            if (logtable == null)
            {
                return NotFound();
            }
            else
            {
                LogTable = logtable;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logtable = await _context.LogTables.FindAsync(id);
            if (logtable != null)
            {
                LogTable = logtable;
                _context.LogTables.Remove(LogTable);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
