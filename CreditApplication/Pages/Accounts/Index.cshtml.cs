using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CreditApplication.Data;
using CreditApplication.Models;

namespace CreditApplication.Pages.Accounts
{
    public class IndexModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public IndexModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CreditApplication.Models.Account> Account { get;set; } = default!;

        public AccountRole? SearchAccountRole { get; set; }

        public async Task OnGetAsync()
        {
            Account = await _context.Accounts
                .Include(a => a.Client).ToListAsync();
        }
    }
}
