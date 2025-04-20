using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CreditApplication.Data;
using CreditApplication.Models;
using System.Net;

namespace CreditApplication.Pages.ApplicationForm
{
    public class Step2Model : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public Step2Model(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ClientAddress ClientAddress { get; set; }

        [TempData]
        public int ClientId { get; set; }

        public IActionResult OnGet()
        {
            TempData.Keep("ClientId");
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid) return Page();

            ClientAddress.ClientID = ClientId;
            _context.ClientAddresses.Add(ClientAddress);
            await _context.SaveChangesAsync();


            return RedirectToPage("Step3_Financials");
        }
    }
}
