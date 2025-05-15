using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CreditApplication.Data;
using CreditApplication.Models;
using System.Drawing.Printing;
using Microsoft.Data.SqlClient;

namespace CreditApplication.Pages.Clients
{
    public class IndexModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public IndexModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        private const int PageSize = 20;

        public IList<Client> Client { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SearchClientId { get; set; }


        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }


        [BindProperty(SupportsGet = true)]
        public string SearchEGN { get; set; }


        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; }
        public string ClientIdSort { get; set; }
        public string NameSort { get; set; }


        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; private set; }


        public async Task OnGetAsync()
        {
            ClientIdSort = String.IsNullOrEmpty(SortOrder) ? "id_desc" : "";
            NameSort = SortOrder == "Name" ? "name_desc" : "Name";

            var query = _context.Clients.AsQueryable();

            if (SearchClientId.HasValue)
            {
                query = query.Where(c => c.ID == SearchClientId.Value);
            }

            if (!string.IsNullOrEmpty(SearchName))
            {
                query = query.Where(c => c.FirstName.Contains(SearchName) || c.LastName.Contains(SearchName));
            }

            if (!string.IsNullOrEmpty(SearchEGN))
            {
                query = query.Where(c => c.EGN.Contains(SearchEGN));
            }


            switch (SortOrder)
            {
                case "id_desc":
                    query = query.OrderByDescending(c => c.ID);
                    break;
                case "Name":
                    query = query.OrderBy(c => c.FirstName).ThenBy(c => c.LastName);
                    break;
                case "name_desc":
                    query = query.OrderByDescending(c => c.FirstName).ThenByDescending(c => c.LastName);
                    break;
                default:
                    query = query.OrderBy(c => c.ID);
                    break;
            }

            var count = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            Client = await query
                 .Skip((PageIndex - 1) * PageSize)
                 .Take(PageSize)
                 .ToListAsync();

            //Client = await _context.Clients
            //   // .Include(c => c.ClientAddress)
            //    .ToListAsync();
        }
    }
}
