using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CreditApplication.Data;
using CreditApplication.Models;

namespace CreditApplication.Pages.Accounts
{
    public class EditModel : PageModel
    {
        private readonly CreditApplicationDbContext _context;

        public EditModel(CreditApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CreditApplication.Models.Account Account { get; set; } = default!;
        private void PopulateDropdowns()
        {
            var selectedRole = Account != null ? (int)Account.Role : (int?)null;
            var selectedClientID = Account?.ClientID;

            ViewData["Roles"] = new SelectList(
                Enum.GetValues(typeof(AccountRole))
                    .Cast<AccountRole>()
                    .Select(r => new { Id = (int)r,
                        Name = r switch
                        {
                            AccountRole.Client => "Клиент",
                            AccountRole.Employee => "Служител",
                            AccountRole.Admin => "Администратор",
                            _ => r.ToString()
                        }
                    }),
                "Id", "Name",
                selectedRole
            );

            ViewData["ClientID"] = new SelectList(
                _context.Clients,
                "ID", "EGN",
                selectedClientID
            );
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var account = await _context.Accounts.FirstOrDefaultAsync(m => m.ID == id);
            if (account == null) return NotFound();

            Account = account;

            PopulateDropdowns();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return Page();
            }

            _context.Attach(Account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(Account.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AccountExists(int id)
            => _context.Accounts.Any(e => e.ID == id);
    }
}
