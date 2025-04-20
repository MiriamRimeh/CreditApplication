using CreditApplication.Data;
using CreditApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CreditApplication.Pages.ApplicationForm
{
    public class EndPageModel : PageModel
    {
        private readonly CreditApplicationDbContext _context;

        public EndPageModel(CreditApplicationDbContext context)
        {
            _context = context;
        }

        [TempData]
        public int ClientId { get; set; }

        public Client Client { get; set; }
        public ClientAddress Address { get; set; }
        public ClientFinancial Financial { get; set; }
        public Credit Credit { get; set; }

        public string CreditStatusDescription { get; set; }
        public string ClientEmploymentType { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Client = await _context.Clients.FindAsync(ClientId);
            Address = await _context.ClientAddresses.FirstOrDefaultAsync(a => a.ClientID == ClientId);
            Financial = await _context.ClientFinancials.FirstOrDefaultAsync(f => f.ClientID == ClientId);
            Credit = await _context.Credits.FirstOrDefaultAsync(c => c.ClientID == ClientId);

            if (Credit != null)
            {
                CreditStatusDescription = await _context.Nomenclatures
                    .Where(n => n.NomCode == Credit.Status)
                    .Select(n => n.Description)
                    .FirstOrDefaultAsync();

                ClientEmploymentType = await _context.Nomenclatures
                    .Where(n => n.NomCode == Financial.EmploymentType)
                    .Select(n => n.Description)
                    .FirstOrDefaultAsync();
            }

            return Page();
        }
    }
}
