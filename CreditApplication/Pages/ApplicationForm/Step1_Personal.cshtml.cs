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
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace CreditApplication.Pages.ApplicationForm
{
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

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userIdClaim)
                && int.TryParse(userIdClaim, out var userId))
            {
                var account = await _context.Accounts
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.ID == userId);

                if (account?.ClientID != null)
                {
                    ClientId = account.ClientID.Value;
                    Client = await _context.Clients
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.ID == ClientId)
                        ?? new Client();
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {          
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null ||
                !int.TryParse(userIdClaim, out var userId))
                return Forbid();

            var account = await _context.Accounts
                .Include(a => a.Client)
                .FirstOrDefaultAsync(a => a.ID == userId);

            if (account == null)
                return Forbid();

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

            if (string.IsNullOrWhiteSpace(Client.EGN) ||
               !Regex.IsMatch(Client.EGN, @"^\d{10}$"))
            {
                ModelState.AddModelError(
                    "Client.EGN",
                    "ЕГН трябва да съдържа точно 10 цифри."
                );
            }
            else
            {
                bool exists = await _context.Clients
                    .AnyAsync(c => c.EGN == Client.EGN && c.ID != account.ClientID);

                if (exists)
                {
                    ModelState.AddModelError(
                        "Client.EGN",
                        "Вече съществува клиент с това ЕГН."
                    );
                }
            }

            if (!ModelState.IsValid) return Page();

            if (account.ClientID == null)
            {
                _context.Clients.Add(Client);
                await _context.SaveChangesAsync();
                //TempData.Keep("ClientId");

                account.ClientID = Client.ID;
                ClientId = Client.ID;

                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();
            }
            else
            {
                var existing = account.Client!;
                existing.FirstName = Client.FirstName;
                existing.LastName = Client.LastName;
                existing.EGN = Client.EGN;
                existing.PhoneNumber = Client.PhoneNumber;

                _context.Clients.Update(existing);
                await _context.SaveChangesAsync();

                ClientId = existing.ID;
            }

            return RedirectToPage("Step2_Address",new { clientId = ClientId });
        }
    }
}
