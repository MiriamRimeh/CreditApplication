using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CreditApplication.Data;
using CreditApplication.Models;

namespace CreditApplication.Pages.Clients
{
    public class IndexModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public IndexModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Client> Client { get; set; }


        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchEGN { get; set; }
        public async Task OnGetAsync()
        {
            var query = _context.Clients.AsQueryable();

            if (!string.IsNullOrEmpty(SearchName))
            {
                query = query.Where(c => c.FirstName.Contains(SearchName) || c.LastName.Contains(SearchName));
            }

            if (!string.IsNullOrEmpty(SearchEGN))
            {
                query = query.Where(c => c.EGN.Contains(SearchEGN));
            }
            Client = await query.ToListAsync();

            //Client = await _context.Clients
            //   // .Include(c => c.ClientAddress)
            //    .ToListAsync();
        }
    }
}
