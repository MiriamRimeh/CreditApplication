using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CreditApplication.Data;
using CreditApplication.Models;

namespace CreditApplication.Pages.ClientFinancials
{
    public class CreateModel : PageModel
    {
        private readonly CreditApplicationDbContext _context;

        public CreateModel(CreditApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ClientFinancial ClientFinancial { get; set; } = default!;

        public SelectList ClientList { get; set; } = default!;

        public SelectList EmploymentTypes { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            PopulateClients();
            await LoadEmploymentTypesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            PopulateClients();
            await LoadEmploymentTypesAsync();
  

            _context.ClientFinancials.Add(ClientFinancial);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private void PopulateClients()
        {
            ClientList = new SelectList(
                _context.Clients
                        .OrderBy(c => c.EGN)
                        .Select(c => new { c.ID, c.EGN }),
                "ID", "EGN");
        }

        private async Task LoadEmploymentTypesAsync()
        {
            var list = await _context.Nomenclatures
                                     .Where(n => n.NomCode >= 301 && n.NomCode <= 306)
                                     .OrderBy(n => n.Description)
                                     .ToListAsync();

            EmploymentTypes = new SelectList(list, "NomCode", "Description");
        }
    }
}
