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


        [TempData]
        public string? StatusMessage { get; set; }
        public int PageSize = 20;

        [BindProperty(SupportsGet = true)]
        public int? CreditId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? SearchPayedOnDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchOperationType { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SearchOperationId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; }
        public string SortId { get; set; }
        public string CreditIDSort { get; set; }
        public string OperationTypeSort { get; set; }
        public string PayedOnDateSort { get; set; }
        public string PayedAmountSort { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; private set; }

        public IList<FinancialOperation> FinancialOperation { get; set; } = default!;


        public async Task OnGetAsync()
        {
            SortId = String.IsNullOrEmpty(SortOrder) ? "SortId" : "";
            CreditIDSort = SortOrder == "CreditIDSort" ? "credit_desc" : "CreditIDSort";
            OperationTypeSort = SortOrder == "OperationType" ? "otype_desc" : "OperationType";
            PayedOnDateSort = SortOrder == "PayedOnDate" ? "payedon_desc" : "PayedOnDate";
            PayedAmountSort = SortOrder == "PayedAmount" ? "amount_desc" : "PayedAmount";

            var query = _context.FinancialOperations
                                .Include(f => f.Credit)
                                .Include(f=> f.OperationTypeNomenclature)
                                .AsQueryable();

            if (SearchOperationId.HasValue)
                query = query.Where(f => f.ID == SearchOperationId.Value);

            if (CreditId.HasValue)
                query = query.Where(f => f.CreditID == CreditId.Value);

            if (SearchPayedOnDate.HasValue)
            {
                var d = SearchPayedOnDate.Value.Date;
                query = query.Where(f => f.PayedOnDate.HasValue
                                      && f.PayedOnDate.Value.Date == d);
            }

            if (!string.IsNullOrWhiteSpace(SearchOperationType))
                query = query.Where(f => f.OperationTypeNomenclature != null && EF.Functions.Like(f.OperationTypeNomenclature.Description, $"%{SearchOperationType}%"));


            switch (SortOrder)
            {
                case "credit_desc":
                    query = query.OrderByDescending(f => f.CreditID);
                    break;
                case "OperationType":
                    query = query.OrderBy(f => f.OperationType);
                    break;
                case "otype_desc":
                    query = query.OrderByDescending(f => f.OperationType);
                    break;
                case "PayedOnDate":
                    query = query.OrderBy(f => f.PayedOnDate);
                    break;
                case "payedon_desc":
                    query = query.OrderByDescending(f => f.PayedOnDate);
                    break;
                case "PayedAmount":
                    query = query.OrderBy(f => f.PayedAmount);
                    break;
                case "amount_desc":
                    query = query.OrderByDescending(f => f.PayedAmount);
                    break;
                case "SortId":
                    query = query.OrderBy(f => f.ID);
                    break;
                case "CreditIDSort":
                    query = query.OrderBy(f => f.CreditID);
                    break;
                default:
                    query = query.OrderByDescending(f => f.ID);
                    break;
            }

            var count = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            FinancialOperation = await query
                .Skip((PageIndex - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostStornoAsync(int id)
        {
            var originalOp = await _context.FinancialOperations.FindAsync(id);
            if (originalOp == null) return NotFound();

            var stornoOp = new FinancialOperation
            {
                CreditID = originalOp.CreditID,
                PayedOnDate = originalOp.PayedOnDate,
                PayedAmount = -originalOp.PayedAmount,
                OperationType = 203,
                RepaymentPlanID = originalOp.ID
            };
            _context.FinancialOperations.Add(stornoOp);

            if (originalOp.RepaymentPlanID.HasValue)
            {
                var rp = await _context.RepaymentPlans.FindAsync(originalOp.RepaymentPlanID.Value);
                if (rp != null)
                {
                    rp.PayedOnDate = null;
                    _context.RepaymentPlans.Update(rp);
                }
            }


            await _context.SaveChangesAsync();
            StatusMessage = "Операцията беше сторнирана успешно.";
            return RedirectToPage();
        }
    }
}
