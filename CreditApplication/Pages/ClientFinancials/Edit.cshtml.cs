using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CreditApplication.Data;
using CreditApplication.Models;

namespace CreditApplication.Pages.ClientFinancials
{
    public class EditModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public EditModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ClientFinancial ClientFinancial { get; set; } = default!;

        public SelectList EmploymentTypes { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nomList = await _context.Nomenclatures
                .AsNoTracking()
                .Where(n => n.NomCode >= 301 && n.NomCode <= 306)
                .OrderBy(n => n.Description)
                .ToListAsync();

            var clientfinancial =  await _context.ClientFinancials.FirstOrDefaultAsync(m => m.ID == id);
            if (clientfinancial == null)
            {
                return NotFound();
            }
            ClientFinancial = clientfinancial;
           ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "EGN");
            return Page();
        }

        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ClientFinancial).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientFinancialExists(ClientFinancial.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            if (!User.IsInRole("Client"))
            {
                return RedirectToPage("./Index");
            }
            else
            {
                return RedirectToPage("/Accounts/Profile");
            }
        }

        private bool ClientFinancialExists(int id)
        {
            return _context.ClientFinancials.Any(e => e.ID == id);
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
