using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CreditApplication.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public decimal Amount { get; set; } = 300;
        [BindProperty]
        public int PeriodMonths { get; set; } = 5;

        public void OnGet()
        {

        }
    }
}
