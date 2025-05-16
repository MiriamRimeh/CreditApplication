using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CreditApplication.Data;
using CreditApplication.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CreditApplication.Pages.ApplicationForm
{
    [Authorize]
    public class Step1Model : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public Step1Model(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Client Client { get; set; }

        [TempData]
        public int ClientId { get; set; }

        public IActionResult OnGet() => Page();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            _context.Clients.Add(Client);
            await _context.SaveChangesAsync();
            ClientId = Client.ID;

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userIdClaim)
                && int.TryParse(userIdClaim, out var userId))
            {
                var account = await _context.Accounts.FindAsync(userId);
                if (account != null && account.ClientID == null)
                {
                    account.ClientID = Client.ID;
                    _context.Accounts.Update(account);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("Step2_Address");
        }
    }
}
