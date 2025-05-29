using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CreditApplication.Data;
using CreditApplication.Models;
using System.Diagnostics;

namespace CreditApplication.Pages.Accounts
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly CreditApplicationDbContext _context;

        public CreateModel(CreditApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public SelectList RoleSelectList { get; set; }

        public class InputModel
        {
            [Required, EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required, StringLength(100, MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required, Compare("Password")]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Role")]
            public AccountRole Role { get; set; }

            [Display(Name = "Active")]
            public bool IsActive { get; set; } = true;
        }

        public IActionResult OnGet()
        {

            RoleSelectList = new SelectList(
                Enum.GetValues(typeof(AccountRole))
            .Cast<AccountRole>()
                    .Select(r => new {
                        Value = r,
                        Text = r switch
                        {
                            AccountRole.Client => "Клиент",
                            AccountRole.Employee => "Служител",
                            AccountRole.Admin => "Администратор",
                            _ => r.ToString()
                        }
                    }),
                "Value", "Text");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            RoleSelectList = new SelectList(
                Enum.GetValues(typeof(AccountRole))
            .Cast<AccountRole>()
                    .Select(r => new { Value = r, Text = r switch
                    {
                        AccountRole.Client => "Клиент",
                        AccountRole.Employee => "Служител",
                        AccountRole.Admin => "Администратор",
                        _ => r.ToString()
                    }
                }),
                "Value", "Text");

            if (!ModelState.IsValid)
                return Page();

            if (await _context.Accounts
                    .AnyAsync(a => a.Username == Input.Email))
            {
                ModelState.AddModelError(string.Empty,
                    "This email is already registered.");
                return Page();
            }

            using var derive = new Rfc2898DeriveBytes(
                Input.Password,
                saltSize: 16,
                iterations: 100_000,
                HashAlgorithmName.SHA256);

            var salt = derive.Salt;
            var hash = derive.GetBytes(32);

            var account = new CreditApplication.Models.Account
            {
                Username = Input.Email,
                PasswordSalt = salt,
                PasswordHash = hash,
                Role = Input.Role,
                IsActive = Input.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
