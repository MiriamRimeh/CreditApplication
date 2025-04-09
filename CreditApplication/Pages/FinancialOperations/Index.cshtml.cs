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
    public class IndexModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public IndexModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        public IList<FinancialOperation> FinancialOperation { get;set; } = default!;

        public async Task OnGetAsync()
        {
            FinancialOperation = await _context.FinancialOperations
                .Include(f => f.Credit).ToListAsync();
        }
    }
}
