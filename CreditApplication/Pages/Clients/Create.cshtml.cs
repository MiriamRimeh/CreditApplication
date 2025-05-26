using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CreditApplication.Data;
using CreditApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace CreditApplication.Pages.Clients
{
    public class CreateModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public CreateModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Client Client { get; set; } = default!;


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
                    .AnyAsync(c => c.EGN == Client.EGN);

                if (exists)
                {
                    ModelState.AddModelError(
                        "Client.EGN",
                        "Вече съществува клиент с това ЕГН."
                    );
                }
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Clients.Add(Client);
            await _context.SaveChangesAsync();

            if (!User.IsInRole("Client"))
            {
                return RedirectToPage("./Index");
            }
            else
            {
                return RedirectToPage("/Accounts/Profile");
            }
        }
    }
}
