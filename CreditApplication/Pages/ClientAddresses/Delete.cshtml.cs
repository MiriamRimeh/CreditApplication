using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CreditApplication.Data;
using CreditApplication.Models;

namespace CreditApplication.Pages.ClientAddresses
{
    public class DeleteModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public DeleteModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ClientAddress ClientAddress { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientaddress = await _context.ClientAddresses.FirstOrDefaultAsync(m => m.ID == id);

            if (clientaddress == null)
            {
                return NotFound();
            }
            else
            {
                ClientAddress = clientaddress;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientaddress = await _context.ClientAddresses.FindAsync(id);
            if (clientaddress != null)
            {
                ClientAddress = clientaddress;
                _context.ClientAddresses.Remove(ClientAddress);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
