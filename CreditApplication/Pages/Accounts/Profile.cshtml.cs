using CreditApplication.Data;
using CreditApplication.Models;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CreditApplication.Pages.Accounts
{
    public class ProfileModel : PageModel
    {
        private readonly CreditApplicationDbContext _context;
        public ProfileModel(CreditApplicationDbContext context) => _context = context;

        public RepaymentPlan? NextInstallment { get; set; }
        public Models.Account Account { get; set; }
        public Client Client { get; set; }
        public ClientAddress Address { get; set; }
        public ClientFinancial Financials { get; set; }

        public List<Credit> PendingCredits { get; set; }
        public List<Credit> ActiveCredits { get; set; }
        public List<Credit> CompletedCredits { get; set; }

        public List<Credit> RejectedCredits { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out var userId))
                return RedirectToPage("/Entrance/Login");

            Account = await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.ID == userId && a.Role == AccountRole.Client);

            if (Account?.ClientID == null)
                return RedirectToPage("/Accounts/Profile");

            Client = await _context.Clients
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(c => c.ID == Account.ClientID.Value);

           
            Address = await _context.ClientAddresses
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.ClientID == Account.ClientID.Value)
                ?? new ClientAddress();

            Financials = await _context.ClientFinancials
                                .Include(f => f.EmploymentTypeNomenclature)
                                .AsNoTracking()
                                .FirstOrDefaultAsync(f => f.ClientID == Account.ClientID.Value) ?? new ClientFinancial();

            var clientId = Account.ClientID;

            NextInstallment = await _context.RepaymentPlans
                .Include(r => r.Credit)
                .Where(r => r.Credit.ClientID == clientId
                            && (r.isPaid == false || r.isPaid == null)
                            && r.InstallmentDate >= DateOnly.FromDateTime(DateTime.Today))
                .OrderBy(r => r.InstallmentDate)
                .FirstOrDefaultAsync();

            var creditsQuery = _context.Credits
                .Where(c => c.ClientID == Account.ClientID.Value)
                .Include(c => c.StatusNavigation)
                .AsNoTracking();

            PendingCredits = await creditsQuery
                .Where(c => c.StatusNavigation != null && c.StatusNavigation.NomCode == 101) // Очакващ решение
                .ToListAsync();

            ActiveCredits = await creditsQuery
                .Where(c => c.StatusNavigation != null && c.StatusNavigation.NomCode == 102) // Активен
                .ToListAsync();

            CompletedCredits = await creditsQuery
                .Where(c => c.StatusNavigation != null && c.StatusNavigation.NomCode == 103) // Приключен
                .ToListAsync();

            RejectedCredits = await creditsQuery
            .Where(c => c.StatusNavigation != null && c.StatusNavigation.NomCode == 104) // Приключен
            .ToListAsync();

            return Page();
        }
    }
}
