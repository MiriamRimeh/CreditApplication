using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CreditApplication.Data;
using CreditApplication.Models;
using Microsoft.AspNetCore.Authorization;

namespace CreditApplication.Pages.Credits
{
    //[Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly CreditApplicationDbContext _context;

        public CreateModel(CreditApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Credit Credit { get; set; } = default!;

        public SelectList ClientList { get; set; } = default!;

        private void PopulateClients()
        {
            ClientList = new SelectList(
                _context.Clients
                        .OrderBy(c => c.EGN)
                        .Select(c => new { c.ID, c.EGN }),
                "ID", "EGN");
        }

        public IActionResult OnGet()
        {
            PopulateClients();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            PopulateClients();

            if (Credit.CreditAmount < 300 || Credit.CreditAmount > 5000)
            {
                ModelState.AddModelError(
                    "Credit.CreditAmount",
                    "Сумата на кредита трябва да бъде между 300 лв и 5000 лв.");
                return Page();
            }
            if (Credit.CreditPeriod < 5 || Credit.CreditPeriod > 24)
            {
                ModelState.AddModelError(
                   "Credit.CreditPeriod",
                    "Периодът на кредита трябва да е между 5 и 24 месеца.");
                return Page();
            }
            if (Credit.ClientID == 0)
            {
                ModelState.AddModelError(
                    "Credit.ClientID",
                    "Моля изберете клиент от списъка.");
                return Page();
            }

            Credit.CreatedOn = DateTime.Now;
            Credit.ModifiedOn = DateTime.Now;
            Credit.InterestRate = 0.40M;
            Credit.Status = 101;

            Credit.TotalCreditAmount = Credit.CreditAmount + Credit.CreditAmount * Credit.InterestRate;
            Credit.MonthlyInstallment = Credit.TotalCreditAmount / Credit.CreditPeriod;

            _context.Credits.Add(Credit);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Credits/Index");
        }
    }
}