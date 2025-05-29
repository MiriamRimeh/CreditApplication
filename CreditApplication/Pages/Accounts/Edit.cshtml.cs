using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CreditApplication.Data;
using CreditApplication.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

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


        [BindProperty, DataType(DataType.Password)]
        [Display(Name = "Нова парола")]
        public string? NewPassword { get; set; }

        [BindProperty, DataType(DataType.Password)]
        [Display(Name = "Повтори паролата")]
        [Compare(nameof(NewPassword),
         ErrorMessage = "Паролите не съвпадат.")]
        public string? ConfirmPassword { get; set; }



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
            //_context.Attach(Account);
            //var entry = _context.Entry(Account);
            //entry.State = EntityState.Modified;

            //entry.Property(a => a.PasswordHash).IsModified = false;
            //entry.Property(a => a.PasswordSalt).IsModified = false;
            //entry.Property(a => a.CreatedAt).IsModified = false;

            PopulateDropdowns();

            if (!ModelState.IsValid)
                return Page();

            var acct = await _context.Accounts
                          .FirstOrDefaultAsync(a => a.ID == Account.ID);
            if (acct == null) return NotFound();

            acct.Username = Account.Username;
            acct.Role = Account.Role;
            acct.IsActive = Account.IsActive;
            acct.ClientID = Account.Role == AccountRole.Client
                             ? Account.ClientID
                             : null;

            if (!string.IsNullOrEmpty(NewPassword))
            {
                using var derive = new Rfc2898DeriveBytes(
                    NewPassword,
                    saltSize: 16,
                    iterations: 100_000,
                    HashAlgorithmName.SHA256);

                acct.PasswordSalt = derive.Salt;
                acct.PasswordHash = derive.GetBytes(32);
            }


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
