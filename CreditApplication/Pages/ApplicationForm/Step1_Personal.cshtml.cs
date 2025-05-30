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

        [BindProperty]
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

            ClientId = account.ClientID ?? 0;

            if (string.IsNullOrWhiteSpace(Client.EGN) ||
               !Regex.IsMatch(Client.EGN, @"^\d{10}$"))
            {
                ModelState.AddModelError(
                    "Client.EGN",
                    "ЕГН трябва да съдържа точно 10 цифри."
                );
                return Page();
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
                    return Page();
                }
            }

            if (Client.IDValidityDate < Client.IDIssueDate)
            {
                ModelState.AddModelError(
                    "Client.IDValidityDate",
                    "Дата на валидност трябва да бъде след датата на издаване."
                );
                return Page();
            }

            if (!(Client.IDValidityDate == Client.IDIssueDate.AddYears(10)))
            {
                ModelState.AddModelError(
                    "Client.IDValidityDate",
                    "Дата на валидност трябва да бъде 10 години след датата на издаване."
                );
                return Page();
            }

            if (Client.IDValidityDate < DateTime.Today)
            {
                ModelState.AddModelError(
                    "Client.IDValidityDate",
                    "Личната Ви карта е изтекла."
                );
                return Page();
            }


            var egn = Client.EGN;
            int yy = int.Parse(egn.Substring(0, 2));
            int mm = int.Parse(egn.Substring(2, 2));
            int dd = int.Parse(egn.Substring(4, 2));

            int year, month;
            if (mm >= 1 && mm <= 12)
            {
                
                year = 1900 + yy;
                month = mm;
            }
            else if (mm >= 21 && mm <= 32)
            {
                
                year = 1800 + yy;
                month = mm - 20;
            }
            else if (mm >= 41 && mm <= 52)
            {
                
                year = 2000 + yy;
                month = mm - 40;
            }
            else
            {
                ModelState.AddModelError(
                    "Client.EGN",
                    "Невалиден месец в ЕГН."
                );
                return Page();
            }

            DateTime birthDate;
            try
            {
                birthDate = new DateTime(year, month, dd);
            }
            catch
            {
                ModelState.AddModelError(
                    "Client.EGN",
                    "Невалидна дата в ЕГН."
                );
                return Page();
            }

            DateTime cutoff = DateTime.Today.AddYears(-18);
            if (birthDate > cutoff)
            {
                ModelState.AddModelError(
                    "Client.EGN",
                    "Клиентът трябва да е навършил поне 18 години."
                );
                return Page();
            }

            //if (!ModelState.IsValid)
            //    return Page();

            if (account.ClientID == null)
            {
                _context.Clients.Add(Client);
                await _context.SaveChangesAsync();
                

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
