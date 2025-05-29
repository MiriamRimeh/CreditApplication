using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CreditApplication.Pages.Accounts
{
    public class NoProfileModel : PageModel
    {
        public Models.Account Account { get; set; }
        public void OnGet()
        {
            Account = new Models.Account
            {
                Username = User.Identity?.Name ?? "Unknown User"
            };
        }
    }
}
