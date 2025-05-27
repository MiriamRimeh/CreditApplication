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

namespace CreditApplication.Pages.ClientAddresses
{
    public class EditModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public EditModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ClientAddress ClientAddress { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientaddress =  await _context.ClientAddresses.FirstOrDefaultAsync(m => m.ID == id);
           
            if (clientaddress == null)
            {
                return NotFound();
            }

            ClientAddress = clientaddress;
            ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "EGN");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            //_context.Attach(ClientAddress).State = EntityState.Modified;

            var addressToUpdate = await _context.ClientAddresses
                                        .FirstOrDefaultAsync(a => a.ID == ClientAddress.ID);

            if (addressToUpdate == null)
            {
                return NotFound();
            }

            _context.Entry(addressToUpdate)
                    .CurrentValues
                    .SetValues(ClientAddress);

            _context.Entry(addressToUpdate).Property(a => a.CreatedOn).IsModified = false;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientAddressExists(ClientAddress.ID))
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

        private bool ClientAddressExists(int id)
        {
            return _context.ClientAddresses.Any(e => e.ID == id);
        }
    }
}
