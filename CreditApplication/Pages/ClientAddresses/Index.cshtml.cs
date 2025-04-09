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
    public class IndexModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public IndexModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ClientAddress> ClientAddress { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ClientAddress = await _context.ClientAddresses
                .Include(c => c.Client).ToListAsync();
        }
    }
}
