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

        public IActionResult OnGet()
        {
            TempData.Keep("ClientId");
            _ = LoadEmploymentTypesAsync();
            return Page();
        }

        [BindProperty]
        public ClientFinancial ClientFinancial { get; set; } = default!;
        [TempData]
        public int ClientId { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid) return Page();

            ClientFinancial.ClientID = ClientId;
            _context.ClientFinancials.Add(ClientFinancial);
            await _context.SaveChangesAsync();

            return RedirectToPage("Step4_Credit");
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
