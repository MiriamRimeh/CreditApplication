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

        private const int PageSize = 20;

        public IList<Credit> Credit { get;set; } = default!;


        [BindProperty(SupportsGet = true)]
        public int? SearchClientId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchStatus { get; set; }


        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; }
        public string CreditIdSort { get; set; }
        public string BeginDateSort { get; set; }
        public string EndDateSort { get; set; }
        public string CreditAmountSort { get; set; }
        public string PeriodSort { get; set; }


        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; private set; }


        public async Task OnGetAsync()
        {
            CreditIdSort = String.IsNullOrEmpty(SortOrder) ? "id" : "";
            BeginDateSort = SortOrder == "CreditBeginDate" ? "creditBeginDate_desc" : "CreditBeginDate";
            CreditAmountSort = SortOrder == "Amount" ? "amount_desc" : "Amount";
            EndDateSort = SortOrder == "EndDate" ? "end_desc" : "EndDate";
            PeriodSort = SortOrder == "CreditPeriod" ? "creditPeriod_desc" : "CreditPeriod";

            var query = _context.Credits
                            .Include(c => c.StatusNavigation)
                            .AsQueryable();

            if (SearchClientId.HasValue)
            {
                query = query.Where(c => c.ClientID == SearchClientId.Value);
            }

            if (!string.IsNullOrWhiteSpace(SearchStatus))
            {
                   query = query.Where(c => c.StatusNavigation != null && EF.Functions.Like(c.StatusNavigation.Description, $"%{SearchStatus}%")
                );
            }

            switch (SortOrder)
            {
                case "id":
                    query = query.OrderBy(c => c.ID);
                    break;
                case "CreditBeginDate":
                    query = query.OrderBy(c => c.CreditBeginDate);
                    break;
                case "creditBeginDate_desc":
                    query = query.OrderByDescending(c => c.CreditBeginDate);
                    break;
                case "Amount":
                    query = query.OrderBy(c => c.CreditAmount);
                    break;
                case "amount_desc":
                    query = query.OrderByDescending(c => c.CreditAmount);
                    break;
                case "EndDate":
                    query = query.OrderBy(c => c.CreditEndDate);
                    break;
                case "end_desc":
                    query = query.OrderByDescending(c => c.CreditEndDate);
                    break;
                case "CreditPeriod":
                    query = query.OrderBy(c => c.CreditPeriod);
                    break;
                case "creditPeriod_desc":
                    query = query.OrderByDescending(c => c.CreditPeriod);
                    break;
                default:
                    query = query.OrderByDescending(c => c.ID);
                    break;
            }

            var count = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            Credit = await query
                 .Skip((PageIndex - 1) * PageSize)
                 .Take(PageSize)
                 .ToListAsync();
        }
    }
}
