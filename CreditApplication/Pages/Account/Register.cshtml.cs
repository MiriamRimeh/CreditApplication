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

            if (await _context.Users.AnyAsync(u => u.Username == Input.Email))
            {
                ModelState.AddModelError(string.Empty, "Имейлът вече е регистриран.");
                return Page();
            }

            // Генериране на сол и хеш
            using var derive = new Rfc2898DeriveBytes(Input.Password, 16, 100_000, HashAlgorithmName.SHA256);
            var salt = derive.Salt; // Convert byte[] to string
            var hash = derive.GetBytes(32); // Convert byte[] to string

            var user = new Users
            {
                Username = Input.Email,
                ClientID = 0,
                PasswordHash = hash, // Now a string
                PasswordSalt = salt, // Now a string
                UserType = 0,      // 0 = клиент
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Account/Login");
        }
    }
}
