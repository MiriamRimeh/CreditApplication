using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CreditApplication.Data;
using CreditApplication.Models;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace CreditApplication.Pages.ApplicationForm
{
    public class Step2Model : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public Step2Model(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ClientAddress ClientAddress { get; set; }

        [TempData]
        public int ClientId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            TempData.Keep("ClientId");
            var existing = await _context.ClientAddresses
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.ClientID == ClientId);
            if (existing != null)
            {
                ClientAddress = existing;
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid) return Page();

            var existing = await _context.ClientAddresses
                .FirstOrDefaultAsync(a => a.ClientID == ClientId);

            if (existing == null)
            {
                ClientAddress.ClientID = ClientId;
                _context.ClientAddresses.Add(ClientAddress);
            }
            else
            {
                existing.City = ClientAddress.City;
                existing.StreetNeighbourhood = ClientAddress.StreetNeighbourhood;
                existing.Number = ClientAddress.Number;
                existing.PostCode = ClientAddress.PostCode;
                _context.ClientAddresses.Update(existing);
            }


            await _context.SaveChangesAsync();
            return RedirectToPage("Step3_Financials");
        }
    }
}
