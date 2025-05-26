// Pages/Step4.cshtml.cs
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CreditApplication.Data;
using CreditApplication.Models;

namespace CreditApplication.Pages
{
    public class Step4Model : PageModel
    {
        private readonly CreditApplicationDbContext _context;
        public Step4Model(CreditApplicationDbContext context)
            => _context = context;

        [BindProperty]
        public Credit Credit { get; set; } = default!;

        [TempData]
        public int ClientId { get; set; }

        public IActionResult OnGet(int clientId)
        {
            TempData.Keep("ClientId");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(Credit.CreditAmount < 300 || Credit.CreditAmount > 5000)
            {
                ModelState.AddModelError(
                    "Credit.CreditAmount",
                    "Сумата на кредита трябва да бъде между 300 лв и 5000 лв."
                );
                return Page();
            }

            if (Credit.CreditPeriod < 5 || Credit.CreditPeriod > 24)
            {
                ModelState.AddModelError(
                    "Credit.CreditPeriod",
                    "Периодът на кредита трябва да е между 5 и 24 месеца."
                );
                return Page();
            }

            Credit.ClientID = ClientId;
            Credit.CreatedOn = DateTime.Now;
            Credit.ModifiedOn = DateTime.Now;
            Credit.InterestRate = 0.4M;
            Credit.Status = 101;

            decimal monthlyRate = Credit.InterestRate.Value / 12M;
            decimal monthly = (Credit.CreditAmount * monthlyRate)
                                / (1 - (decimal)Math.Pow(
                                      (double)(1 + monthlyRate),
                                      -Credit.CreditPeriod.Value));
            decimal total = monthly * Credit.CreditPeriod.Value;

            Credit.MonthlyInstallment = monthly;
            Credit.TotalCreditAmount = total;

            _context.Credits.Add(Credit);
            await _context.SaveChangesAsync();

            return RedirectToPage("EndPage");
        }
    }
}
