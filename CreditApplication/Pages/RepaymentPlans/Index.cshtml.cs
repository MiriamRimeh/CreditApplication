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

        public async Task OnGetAsync()
        {
            //RepaymentPlan = await _context.RepaymentPlans
            //    .Include(r => r.Credit).ToListAsync();

            var query = _context.RepaymentPlans
                            .Include(r => r.Credit)
                            .AsQueryable();

            if (CreditId.HasValue)
            {
                query = query.Where(rp => rp.CreditID == CreditId.Value);
            }

            RepaymentPlan = await query
                .OrderBy(rp => rp.InstallmentNumber)
                .ToListAsync();
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
