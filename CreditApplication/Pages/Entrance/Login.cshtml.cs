using CreditApplication.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace CreditApplication.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly CreditApplicationDbContext _context;

        public LoginModel(CreditApplicationDbContext context)
            => _context = context;

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required, EmailAddress]
            public string Email { get; set; }

            [Required, DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //    return Page();

            var account = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.Username == Input.Email && a.IsActive);

            if (account == null)
            {
                ModelState.AddModelError("Input.Email", "Този имейл не съществува.");
                return Page();
            }

            using var derive = new Rfc2898DeriveBytes(
                Input.Password, account.PasswordSalt, 100_000, HashAlgorithmName.SHA256);
            var hash = derive.GetBytes(32);

            if (!CryptographicOperations.FixedTimeEquals(hash, account.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Невалидна парола.");
                return Page();
            }

            // логваме с ролята като ClaimTypes.Role
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.ID.ToString()),
                new Claim(ClaimTypes.Name, account.Username),
                new Claim(ClaimTypes.Role, account.Role.ToString())
            };

            var identity = new ClaimsIdentity(claims, "Identity.Application");
            var principal = new ClaimsPrincipal(identity);

            var props = new AuthenticationProperties
            {
                IsPersistent = false // Важно: не пазим сесия между рестарти
            };

            await HttpContext.SignInAsync("Identity.Application", principal, props);

            return RedirectToPage("/Index");
        }
    }
}
