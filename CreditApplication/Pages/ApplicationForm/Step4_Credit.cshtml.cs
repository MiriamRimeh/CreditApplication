using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CreditApplication.Data;
using CreditApplication.Models;

namespace CreditApplication.Pages
{
    public class Step4Model : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public Step4Model(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            TempData.Keep("ClientId");
            return Page();
        }

        [BindProperty]
        public Credit Credit { get; set; } = default!;
        [TempData]
        public int ClientId { get; set; }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }



            _context.Credits.Add(Credit);
            await _context.SaveChangesAsync();

            Credit.ClientID = ClientId;
            Credit.CreatedOn = DateTime.Now;
            Credit.ModifiedOn = DateTime.Now;

            return RedirectToPage("EndPage");
        }
    }
}
