using CreditApplication.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

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
            if (!ModelState.IsValid)
                return Page();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == Input.Email && u.IsActive);
            if (user == null)
            {
               // ModelState.AddModelError(string.Empty, "Невалиден имейл или парола.");
                ModelState.AddModelError("Input.Email", "Този имейл не съществува.");
                return Page();
            }

            // Convert the PasswordSalt from string to byte[] before using it
            using var derive = new Rfc2898DeriveBytes(Input.Password, user.PasswordSalt, 100_000, HashAlgorithmName.SHA256);
            var hash = derive.GetBytes(32);

            if (!CryptographicOperations.FixedTimeEquals(hash,user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Невалиден имейл или парола.");
                return Page();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("UserType", user.UserType.ToString())
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return RedirectToPage("/Index");
        }
    }
}
