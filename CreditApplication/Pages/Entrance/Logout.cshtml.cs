using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;

namespace CreditApplication.Pages.Entrance
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            await HttpContext.SignOutAsync("Identity.Application");
            return RedirectToPage("/Entrance/Login");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await HttpContext.SignOutAsync("Identity.Application");
            return RedirectToPage("/Entrance/Login");
        }
    }
}
