﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CreditApplication.Data;
using CreditApplication.Models;

namespace CreditApplication.Pages.Nomenclatures
{
    public class DeleteModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public DeleteModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Nomenclature Nomenclature { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nomenclature = await _context.Nomenclatures.FirstOrDefaultAsync(m => m.NomCode == id);

            if (nomenclature == null)
            {
                return NotFound();
            }
            else
            {
                Nomenclature = nomenclature;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nomenclature = await _context.Nomenclatures.FindAsync(id);
            if (nomenclature != null)
            {
                Nomenclature = nomenclature;
                _context.Nomenclatures.Remove(Nomenclature);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
