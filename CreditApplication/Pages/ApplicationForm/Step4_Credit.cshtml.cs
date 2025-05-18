using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CreditApplication.Data;
using CreditApplication.Models;

namespace CreditApplication.Pages
{
    public class Step4Model : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public Step4Model(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            TempData.Keep("ClientId");
            return Page();
        }

        [BindProperty]
        public Credit Credit { get; set; } = default!;
        [TempData]
        public int ClientId { get; set; }

        public string MonthlyInstallment { get; set; }
        public string TotalCreditAmount { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            Credit.ClientID = ClientId;
            Credit.CreatedOn = DateTime.Now;
            Credit.ModifiedOn = DateTime.Now;
            Credit.InterestRate = 0.4M; // Fixed: Added 'M' suffix to indicate a decimal literal
            Credit.Status = 101; // 101 is the status for "for review" 

            decimal monthlyRate = Credit.InterestRate.Value / 12M; // Ensure consistent use of decimal
            decimal monthly = (Credit.CreditAmount * monthlyRate) / (1 - (decimal)Math.Pow((double)(1 + monthlyRate), -Credit.CreditPeriod.Value));
            decimal total = monthly * Credit.CreditPeriod.Value;

            Credit.MonthlyInstallment = monthly;
            Credit.TotalCreditAmount = total;

            _context.Credits.Add(Credit);
            await _context.SaveChangesAsync();

            return RedirectToPage("EndPage");
        }
    }
}
