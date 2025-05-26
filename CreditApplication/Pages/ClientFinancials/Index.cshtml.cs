using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CreditApplication.Data;
using CreditApplication.Models;
using ClosedXML.Excel;

namespace CreditApplication.Pages.ClientFinancials
{
    public class IndexModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public IndexModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        private const int PageSize = 20;

        public IList<ClientFinancial> ClientFinancial { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public int? SearchClientId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchEGN { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal? SearchMonthlyIncome { get; set; }
        [BindProperty(SupportsGet = true)]
        public decimal? SearchMonthlyExpenses { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchEmploymentType { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; }
        public string ClientIdSort { get; set; }
        public string MonthlyIncomeSort { get; set; }
        public string MonthlyExpensesSort { get; set; }
        public string EmploymentTypeSort { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; private set; }

        public async Task OnGetAsync()
        {
            ClientIdSort = String.IsNullOrEmpty(SortOrder) ? "clientid_desc" : "";
            MonthlyIncomeSort = SortOrder == "monthlyincome" ? "monthlyincome_desc" : "monthlyincome";
            MonthlyExpensesSort = SortOrder == "monthlyexpenses" ? "monthlyexpenses_desc" : "monthlyexpenses";
            EmploymentTypeSort = SortOrder == "employmenttype" ? "employmenttype_desc" : "employmenttype";

            var query = _context.ClientFinancials
                .Include(f => f.EmploymentTypeNomenclature)
                .Include(f => f.Client)
                .AsQueryable();

            if (SearchClientId.HasValue)
                query = query.Where(f => f.ClientID == SearchClientId.Value);
            if (!string.IsNullOrWhiteSpace(SearchEGN))
                query = query.Where(c => c.Client != null
                                      && EF.Functions.Like(c.Client.EGN, $"%{SearchEGN}%"));
            if (SearchMonthlyIncome.HasValue)
                query = query.Where(f => f.MontlyIncome == SearchMonthlyIncome.Value);
            if (SearchMonthlyExpenses.HasValue)
                query = query.Where(f => f.MontlyExpenses == SearchMonthlyExpenses.Value);
            if (!string.IsNullOrEmpty(SearchEmploymentType))
                query = query.Where(f => f.EmploymentTypeNomenclature.Description.Contains(SearchEmploymentType));

            switch (SortOrder)
            {
                case "clientid_desc":
                    query = query.OrderByDescending(f => f.ClientID);
                    break;
                case "monthlyincome":
                    query = query.OrderBy(f => f.MontlyIncome);
                    break;
                case "monthlyincome_desc":
                    query = query.OrderByDescending(f => f.MontlyIncome);
                    break;
                case "monthlyexpenses":
                    query = query.OrderBy(f => f.MontlyExpenses);
                    break;
                case "monthlyexpenses_desc":
                    query = query.OrderByDescending(f => f.MontlyExpenses);
                    break;
                case "employmenttype":
                    query = query.OrderBy(f => f.EmploymentTypeNomenclature.Description);
                    break;
                case "employmenttype_desc":
                    query = query.OrderByDescending(f => f.EmploymentTypeNomenclature.Description);
                    break;
                default:
                    query = query.OrderBy(f => f.ClientID);
                    break;
            }

            var count = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            ClientFinancial = await query
                .Skip((PageIndex - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostExportToExcelAsync()
        {
            var data = await _context.ClientFinancials
                .Include(f => f.EmploymentTypeNomenclature)
                .Include(f => f.Client)
                .ToListAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("ClientFinancials");
            var headers = new[]
            {
            "ClientID", "MonthlyIncome", "MonthlyExpenses", "EmploymentType", "CreatedOn", "ModifiedOn"
        };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
            }
            for (int i = 0; i < data.Count; i++)
            {
                var row = i + 2;
                var f = data[i];
                worksheet.Cell(row, 1).Value = f.ClientID;
                worksheet.Cell(row, 2).Value = f.MontlyIncome;
                worksheet.Cell(row, 3).Value = f.MontlyExpenses;
                worksheet.Cell(row, 4).Value = f.EmploymentTypeNomenclature?.Description ?? string.Empty;
                worksheet.Cell(row, 5).Value = f.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss");
                worksheet.Cell(row, 6).Value = f.ModifiedOn != default
                    ? f.ModifiedOn.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            var fileName = $"ClientFinancials_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }
    }
}
