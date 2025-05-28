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
                .ThenInclude(c => c.ClientFinancials)
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

            var finOp = new FinancialOperation
            {
                CreditID = credit.ID,
                PayedOnDate = DateTime.Now,
                PayedAmount = -(credit.CreditAmount),
                OperationType = 201
            };


            _context.FinancialOperations.Add(finOp);

            await _context.SaveChangesAsync();

            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostRejectAsync(int id)
        {
            var credit = await _context.Credits.FindAsync(id);
            if (credit == null) return NotFound();
            credit.Status = 104;    
            credit.ModifiedOn = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostFinishAsync(int id)
        {
            var credit = await _context.Credits.FindAsync(id);
            if (credit == null) return NotFound();

            credit.Status = 103;
            credit.CreditEndDate = DateTime.Now;
            credit.ModifiedOn = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id });
        }
    }
}
