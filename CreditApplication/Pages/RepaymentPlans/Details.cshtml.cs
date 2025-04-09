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
    public class DetailsModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public DetailsModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        public RepaymentPlan RepaymentPlan { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repaymentplan = await _context.RepaymentPlans.FirstOrDefaultAsync(m => m.ID == id);
            if (repaymentplan == null)
            {
                return NotFound();
            }
            else
            {
                RepaymentPlan = repaymentplan;
            }
            return Page();
        }
    }
}
