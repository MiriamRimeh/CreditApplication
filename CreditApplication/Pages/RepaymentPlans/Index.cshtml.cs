using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CreditApplication.Data;
using CreditApplication.Models;

namespace CreditApplication.Pages.RepaymentPlans
{
    public class IndexModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public IndexModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int? CreditId { get; set; }
        public IList<RepaymentPlan> RepaymentPlan { get;set; } = default!;

        [TempData]
        public string? StatusMessage { get; set; }
        public int PageSize = 20;

        [BindProperty(SupportsGet = true)]
        public int? CreditID { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? SearchInstallmentDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? SearchPayedOnDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; }
        public string CreditIDSort { get; set; }
        public string InstallmentNumberSort { get; set; }
        public string InstallmentDateSort { get; set; }
        public string PayedOnDateSort { get; set; }
        public string AmountSort { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; private set; }

        public async Task OnGetAsync()
        {

            CreditIDSort = String.IsNullOrEmpty(SortOrder) ? "credit_desc" : "";
            InstallmentNumberSort = SortOrder == "InstallmentNumber" ? "installment_desc" : "InstallmentNumber";
            InstallmentDateSort = SortOrder == "InstallmentDate" ? "date_desc" : "InstallmentDate";
            PayedOnDateSort = SortOrder == "PayedOnDate" ? "payedon_desc" : "PayedOnDate";
            AmountSort = SortOrder == "InstallmentAmount" ? "amount_desc" : "InstallmentAmount";

            var query = _context.RepaymentPlans
                            .Include(r => r.Credit)
                            .AsQueryable();

            if (CreditId.HasValue)
            {
                query = query.Where(rp => rp.CreditID == CreditId.Value);
            }

            if (SearchInstallmentDate.HasValue)
            {
                var d = DateOnly.FromDateTime(SearchInstallmentDate.Value);
                query = query.Where(rp => rp.InstallmentDate.HasValue
                                      && rp.InstallmentDate.Value == d);
            }

            if (SearchPayedOnDate.HasValue)
            {
                var d = SearchPayedOnDate.Value.Date;
                query = query.Where(rp => rp.PayedOnDate.HasValue
                                      && rp.PayedOnDate.Value.Date == d);
            }

            // 4) Сортиране
            switch (SortOrder)
            {
                case "credit_desc":
                    query = query.OrderByDescending(rp => rp.CreditID);
                    break;
                case "InstallmentNumber":
                    query = query.OrderBy(rp => rp.InstallmentNumber);
                    break;
                case "installment_desc":
                    query = query.OrderByDescending(rp => rp.InstallmentNumber);
                    break;
                case "InstallmentDate":
                    query = query.OrderBy(rp => rp.InstallmentDate);
                    break;
                case "date_desc":
                    query = query.OrderByDescending(rp => rp.InstallmentDate);
                    break;
                case "PayedOnDate":
                    query = query.OrderBy(rp => rp.PayedOnDate);
                    break;
                case "payedon_desc":
                    query = query.OrderByDescending(rp => rp.PayedOnDate);
                    break;
                case "InstallmentAmount":
                    query = query.OrderBy(rp => rp.InstallmentAmount);
                    break;
                case "amount_desc":
                    query = query.OrderByDescending(rp => rp.InstallmentAmount);
                    break;
                default:
                    query = query.OrderBy(rp => rp.CreditID);
                    break;
            }

            // 5) Пагинация
            var count = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            RepaymentPlan = await query
                .Skip((PageIndex - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            //RepaymentPlan = await query
            //    .OrderBy(rp => rp.InstallmentNumber)
            //    .ToListAsync();
        }

        public async Task<IActionResult> OnPostPayAsync(int id)
        {
            var rp = await _context.RepaymentPlans.FindAsync(id);
            if (rp == null)
                return NotFound();

            // Update the payment date
            rp.PayedOnDate = DateTime.Today;

            var finOp = new FinancialOperation
            {
                CreditID = rp.CreditID,                         
                PayedOnDate = rp.PayedOnDate.Value,                 
                PayedAmount = rp.InstallmentAmount ?? 0m,           
                OperationType = 202                                  
            };

            _context.FinancialOperations.Add(finOp);

            await _context.SaveChangesAsync();


            if (rp.InstallmentDate.HasValue)
            {
                DateTime installmentDate = rp.InstallmentDate.Value.ToDateTime(TimeOnly.MinValue);
                int diff = (DateTime.Today - installmentDate).Days;

                if (diff < 0)
                {
                    StatusMessage = $"Вноската е платена {-diff} дни предварително";
                }
                else if (diff > 0)
                {
                    StatusMessage = $"Вноската е платена с {diff} дни закъснение";
                }
                else
                {
                    StatusMessage = "Вноската е платена днес";
                }
            }
            else
            {
                StatusMessage = "Вноската е маркирана като платена";
            }

            return RedirectToPage(new { CreditId = this.CreditId });
        }
    }
}
