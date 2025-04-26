using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CreditApplication.Data;
using CreditApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace CreditApplication.Pages.Shared
{
    public class _CreditFormModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public _CreditFormModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["EGNList"] = new SelectList(_context.Clients, "ID", "EGN");
        ViewData["Status"] = new SelectList(_context.Nomenclatures, "NomCode", "Description");

            return Page();
        }

        [BindProperty]
        public Credit Credit { get; set; } = default!;

        [BindProperty]
        public string SelectedEgn { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            ViewData["EgnList"] = _context.Clients
                                         .Select(c => c.EGN)
                                         .ToList();

            // Проверка: попълнено ли е ЕГН
            if (string.IsNullOrWhiteSpace(SelectedEgn))
            {
                ModelState.AddModelError(nameof(SelectedEgn), "Моля въведете ЕГН на клиент.");
            }
            else
            {
                // Търсим клиента по ЕГН
                var client = await _context.Clients
                                           .FirstOrDefaultAsync(c => c.EGN == SelectedEgn);
                if (client == null)
                {
                    ModelState.AddModelError(nameof(SelectedEgn), "Няма клиент с това ЕГН.");
                }
                else
                {
                    // Свързваме ClientID със съответния ID от таблицата Clients
                    Credit.ClientID = client.ID;
                }
            }

            Credit.CreatedOn = DateTime.Now;
            Credit.ModifiedOn = DateTime.Now;
            Credit.InterestRate = 0.4M; 
            Credit.Status = 101; 

            Credit.TotalCreditAmount = Credit.CreditAmount + Credit.CreditAmount * Credit.InterestRate;
            Credit.MonthlyInstallment = (Credit.TotalCreditAmount / Credit.CreditPeriod);


            _context.Credits.Add(Credit);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
