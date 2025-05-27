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

namespace CreditApplication.Pages.Clients
{
    public class EditModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public EditModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Client Client { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client =  await _context.Clients.FirstOrDefaultAsync(m => m.ID == id);

            if (client == null)
            {
                return NotFound();
            }

            Client = client;
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (Client.IDValidityDate < Client.IDIssueDate)
            {
                ModelState.AddModelError(
                    "Client.IDValidityDate",
                    "Дата на валидност трябва да бъде след датата на издаване."
                );
            }
            if (!(Client.IDValidityDate == Client.IDIssueDate.AddYears(10)))
            {
                ModelState.AddModelError(
                    "Client.IDValidityDate",
                    "Дата на валидност трябва да бъде 10 години след датата на издаване."
                );
            }



            _context.Attach(Client);
            var entry = _context.Entry(Client);
            entry.State = EntityState.Modified;
            entry.Property(c => c.CreatedOn).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(Client.ID))
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

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.ID == id);
        }
    }
}
