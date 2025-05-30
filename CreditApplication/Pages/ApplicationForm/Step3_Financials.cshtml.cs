using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CreditApplication.Data;
using CreditApplication.Models;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore;

namespace CreditApplication.Pages
{
    public class Step3Model : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public Step3Model(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ClientFinancial Financial { get; set; } = default!;

        [BindProperty]
        public int ClientId { get; set; }

        public SelectList EmploymentTypes { get; set; }

        public async Task<IActionResult> OnGetAsync(int clientId)
        {
            if (!_context.Clients.Any(c => c.ID == clientId))
                return NotFound("Client not found.");
            ClientId = clientId;


            var existing = await _context.ClientFinancials.FirstOrDefaultAsync(f => f.ClientID == ClientId);
            Financial = existing ?? new ClientFinancial();

            var nomList = await _context.Nomenclatures
                .AsNoTracking()
                .Where(n => n.NomCode >= 301 && n.NomCode <= 306)
                .OrderBy(n => n.Description)
                .ToListAsync();

            EmploymentTypes = new SelectList(nomList, "NomCode", "Description");
            ViewData["EmploymentTypes"] = EmploymentTypes;

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!_context.Clients.Any(c => c.ID == ClientId))
            {
                ModelState.AddModelError("", "Invalid Client ID.");
                return Page();
            }

            if(Financial.MontlyIncome == null)
            {
                ModelState.AddModelError(
                    "Financial.MontlyIncome",
                    "Моля, въведете размер на доходи."
                );
                return Page();
            }


            if (Financial.MontlyExpenses == null)
            {
                ModelState.AddModelError(
                    "Financial.MontlyExpenses",
                    "Моля, въведете размер на разходи."
                );
                return Page();
            }



            var existing = await _context.ClientFinancials.FirstOrDefaultAsync(f => f.ClientID == ClientId);

            if (existing == null)
            {
                Financial.ClientID = ClientId;
                _context.ClientFinancials.Add(Financial);
            }
            else
            {
                existing.MontlyIncome = Financial.MontlyIncome;
                existing.MontlyExpenses = Financial.MontlyExpenses;
                existing.EmploymentType = Financial.EmploymentType;
                _context.ClientFinancials.Update(existing);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("Step4_Credit", new { clientId = ClientId });
        }

        private async Task LoadEmploymentTypesAsync()
        {
            var employmentTypes = await _context.Nomenclatures
                .Where(n => n.NomCode >= 301 && n.NomCode <= 306) // Fixed property name from 'ID' to 'NomCode'
                .OrderBy(n => n.Description)
                .ToListAsync();

            ViewData["EmploymentTypes"] = new SelectList(employmentTypes, "NomCode", "Description"); // Updated to match the correct property
        }
    }
}
