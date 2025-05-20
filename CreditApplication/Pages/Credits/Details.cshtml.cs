using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CreditApplication.Data;
using CreditApplication.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CreditApplication.Pages.Credits
{

    public class DetailsModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public DetailsModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        public Credit Credit { get; set; } = default!;

        public FinancialOperation FinancialOperation { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var credit = await _context.Credits
                .Include(c => c.StatusNavigation) 
                .Include(c => c.Client)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (credit == null)
            {
                return NotFound();
            }
            else
            {
                Credit = credit;
            }
            return Page();
        }
        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            var credit = await _context.Credits.FindAsync(id);
            if (credit == null) return NotFound();
            credit.Status = 102;
            credit.CreditBeginDate = DateTime.Now;
            credit.ModifiedOn = DateTime.Now;

            // Correctly initialize the FinancialOperation object
            var finOp = new FinancialOperation
            {
                CreditID = credit.ID,                         // номер на кредита
                PayedOnDate = DateTime.Now,                   // дата на операцията
                PayedAmount = -(credit.CreditAmount),         // усвоена сума
                OperationType = 201                           // код 201 – Усвояване на кредит
            };

            // Add the financial operation to the context
            _context.FinancialOperations.Add(finOp);

            await _context.SaveChangesAsync();

            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostRejectAsync(int id)
        {
            var credit = await _context.Credits.FindAsync(id);
            if (credit == null) return NotFound();
            credit.Status = 104;              // Отхвърлен
            credit.ModifiedOn = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostFinishAsync(int id)
        {
            var credit = await _context.Credits.FindAsync(id);
            if (credit == null) return NotFound();

            credit.Status = 103;              // Приключен
            credit.CreditEndDate = DateTime.Now;
            credit.ModifiedOn = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id });
        }
    }
}
