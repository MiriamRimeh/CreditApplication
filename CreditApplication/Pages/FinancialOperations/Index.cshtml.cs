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

        [TempData]
        public string? StatusMessage { get; set; }
        public async Task OnGetAsync()
        {
            FinancialOperation = await _context.FinancialOperations
                .Include(f => f.Credit).ToListAsync();
        }

        public async Task<IActionResult> OnPostStornoAsync(int id)
        {
            var originalOp = await _context.FinancialOperations.FindAsync(id);
            if (originalOp == null) return NotFound();

            var stornoOp = new FinancialOperation
            {
                CreditID = originalOp.CreditID,
                //PayedOnDate = originalOp.PayedOnDate,
                PayedOnDate = null,
                PayedAmount = -originalOp.PayedAmount,
                OperationType = 203
            };
            _context.FinancialOperations.Add(stornoOp);

            //// 2) Нулира PayedOnDate в RepaymentPlan, за да се активира отново бутон „Плати“
            //var rp = await _context.RepaymentPlans
            //    .FirstOrDefaultAsync(r =>
            //        r.CreditID == originalOp.CreditID
            //        && r.PayedOnDate == originalOp.PayedOnDate
            //        && r.InstallmentAmount == originalOp.PayedAmount
            //    );
            //if (rp != null)
            //{
            //    rp.PayedOnDate = null;
            //    _context.RepaymentPlans.Update(rp);
            //}

            await _context.SaveChangesAsync();
            StatusMessage = "Операцията беше сторнирана успешно.";
            return RedirectToPage();
        }
    }
}
