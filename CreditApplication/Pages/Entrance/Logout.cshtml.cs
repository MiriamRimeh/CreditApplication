using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CreditApplication.Pages.Entrance
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Ако някой посети URL-то чрез браузъра с GET – просто го пренасочваме към Login
            return RedirectToPage("/Entrance/Login");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Излизаме от текущата cookie-аутентикация
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Пренасочваме към страницата за вход
            return RedirectToPage("/Entrance/Login");
        }
    }
}
