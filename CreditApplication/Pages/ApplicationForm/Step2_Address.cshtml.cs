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
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

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
        public ClientAddress Address { get; set; }

        [BindProperty]
        public int ClientId { get; set; }

        public async Task<IActionResult> OnGetAsync(int clientId)
        {
            if (!_context.Clients.Any(c => c.ID == clientId))
                return NotFound("Client not found.");
            ClientId = clientId;

            var existing = await _context.ClientAddresses.FirstOrDefaultAsync(a => a.ClientID == ClientId);
            Address = existing ?? new ClientAddress();

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!_context.Clients.Any(c => c.ID == ClientId))
            {
                ModelState.AddModelError("", "Invalid Client ID.");
                return Page();
            }

            

            var existingAddress = await _context.ClientAddresses.FirstOrDefaultAsync(a => a.ClientID == ClientId);
            if (existingAddress == null)
            {
                Address.ClientID = ClientId;
                _context.ClientAddresses.Add(Address);
            }
            else
            {
                existingAddress.StreetNeighbourhood = Address.StreetNeighbourhood;
                existingAddress.City = Address.City;
                existingAddress.PostCode = Address.PostCode;
                _context.ClientAddresses.Update(existingAddress);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("Step3_Financials", new { clientId = ClientId });
        }
    }
}
