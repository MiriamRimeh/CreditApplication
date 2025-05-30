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
                    .AnyAsync(c => c.EGN == Client.EGN);

                if (exists)
                {
                    ModelState.AddModelError(
                        "Client.EGN",
                        "Вече съществува клиент с това ЕГН."
                    );
                    return Page();
                }
                
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
                    "Клиентът трябва да е навършил 18 години."
                );
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
