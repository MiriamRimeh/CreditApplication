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
    public class IndexModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public IndexModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Credit> Credit { get;set; } = default!;


        [BindProperty(SupportsGet = true)]
        public int? SearchClientId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchStatus { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Credits
                            .Include(c => c.StatusNavigation)
                            .AsQueryable();

            if (SearchClientId.HasValue)
            {
                query = query.Where(c => c.ClientID == SearchClientId.Value);
            }

            if (!string.IsNullOrWhiteSpace(SearchStatus))
            {
                // Convert NomCode to string before using EF.Functions.Like
                query = query.Where(c =>
                    c.StatusNavigation != null &&
                    EF.Functions.Like(c.StatusNavigation.Description, $"%{SearchStatus}%")
                );
            }

            // Sort by creation date
            query = query.OrderByDescending(c => c.CreatedOn);

            Credit = await query.ToListAsync();
        }
    }
}
