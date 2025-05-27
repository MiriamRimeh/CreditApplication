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
    public class DetailsModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public DetailsModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        public ClientAddress ClientAddress { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            ClientAddress = await _context.ClientAddresses
                .Include(ca => ca.Client)
                .FirstOrDefaultAsync(ca => ca.ID == id);

            if (ClientAddress == null)
                return NotFound();

            return Page();
        }
    }
}
