using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CreditApplication.Data;
using CreditApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace CreditApplication.Pages.Credits
{
    public class CreateModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public CreateModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["EgnList"] = _context.Clients
                                              .Select(c => c.EGN)
                                              .ToList();
            return Page();
        }

        [BindProperty]
        public Credit Credit { get; set; } = default!;

        [BindProperty]
        public string SelectedEgn { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

          
        }
    }
}
