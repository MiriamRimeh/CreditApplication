using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CreditApplication.Data;
using CreditApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace CreditApplication.Pages.Account
{
    public class RegisterModel : PageModel
    {

        private readonly CreditApplicationDbContext _context;

        public RegisterModel(CreditApplicationDbContext context)
            => _context = context;

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required, EmailAddress]
            public string Email { get; set; }

            [Required, StringLength(100, MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required, Compare("Password")]
            [DataType(DataType.Password)]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (await _context.Accounts.AnyAsync(u => u.Username == Input.Email))
            {
                ModelState.AddModelError(string.Empty, "Имейлът вече е регистриран.");
                return Page();
            }

            // Генериране на сол и хеш
            using var derive = new Rfc2898DeriveBytes(Input.Password, 16, 100_000, HashAlgorithmName.SHA256);
            var salt = derive.Salt; // Convert byte[] to string
            var hash = derive.GetBytes(32); // Convert byte[] to string

            AccountRole role;
            if (Input.Email.EndsWith("@emp.creditapp.bg", StringComparison.OrdinalIgnoreCase))
            {
                role = AccountRole.Employee;
            }
            else if (Input.Email.EndsWith("@admin.creditapp.bg", StringComparison.OrdinalIgnoreCase))
            {
                role = AccountRole.Admin;
            }
            else
            {
                role = AccountRole.Client;
            }

            bool isEmployee = Input.Email.EndsWith("@unwe.bg",
                                         StringComparison.OrdinalIgnoreCase);

            var account = new CreditApplication.Models.Account
            {
                Username = Input.Email,
                ClientID = null,                   // няма Client профил още
                PasswordSalt = salt,
                PasswordHash = hash,
                Role = role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Entrance/Login");
        }
    }
}
