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

namespace CreditApplication.Pages.Credits
{
    public class IndexModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public IndexModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        private const int PageSize = 20;

        public IList<Credit> Credit { get;set; } = default!;


        [BindProperty(SupportsGet = true)]
        public int? SearchClientId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CreditId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchStatus { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchEGN { get; set; }



        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; }
        public string CreditIdSort { get; set; }
        public string BeginDateSort { get; set; }
        public string EndDateSort { get; set; }
        public string CreditAmountSort { get; set; }
        public string PeriodSort { get; set; }


        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; private set; }


        public async Task OnGetAsync()
        {
            CreditIdSort = String.IsNullOrEmpty(SortOrder) ? "id" : "";
            BeginDateSort = SortOrder == "CreditBeginDate" ? "creditBeginDate_desc" : "CreditBeginDate";
            CreditAmountSort = SortOrder == "Amount" ? "amount_desc" : "Amount";
            EndDateSort = SortOrder == "EndDate" ? "end_desc" : "EndDate";
            PeriodSort = SortOrder == "CreditPeriod" ? "creditPeriod_desc" : "CreditPeriod";

            var query = _context.Credits
                            .Include(c => c.StatusNavigation)
                            .Include(c => c.Client)
                            .AsQueryable();

            if (CreditId.HasValue)
                query = query.Where(f => f.ID == CreditId.Value);

            if (SearchClientId.HasValue)
            {
                query = query.Where(c => c.ClientID == SearchClientId.Value);
            }

            if (!string.IsNullOrWhiteSpace(SearchEGN))
                query = query.Where(c => c.Client != null
                                      && EF.Functions.Like(c.Client.EGN, $"%{SearchEGN}%"));

            if (!string.IsNullOrWhiteSpace(SearchStatus))
            {
                   query = query.Where(c => c.StatusNavigation != null && EF.Functions.Like(c.StatusNavigation.Description, $"%{SearchStatus}%")
                );
            }

            switch (SortOrder)
            {
                case "id":
                    query = query.OrderBy(c => c.ID);
                    break;
                case "CreditBeginDate":
                    query = query.OrderBy(c => c.CreditBeginDate);
                    break;
                case "creditBeginDate_desc":
                    query = query.OrderByDescending(c => c.CreditBeginDate);
                    break;
                case "Amount":
                    query = query.OrderBy(c => c.CreditAmount);
                    break;
                case "amount_desc":
                    query = query.OrderByDescending(c => c.CreditAmount);
                    break;
                case "EndDate":
                    query = query.OrderBy(c => c.CreditEndDate);
                    break;
                case "end_desc":
                    query = query.OrderByDescending(c => c.CreditEndDate);
                    break;
                case "CreditPeriod":
                    query = query.OrderBy(c => c.CreditPeriod);
                    break;
                case "creditPeriod_desc":
                    query = query.OrderByDescending(c => c.CreditPeriod);
                    break;
                default:
                    query = query.OrderByDescending(c => c.ID);
                    break;
            }

            var count = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            Credit = await query
                 .Skip((PageIndex - 1) * PageSize)
                 .Take(PageSize)
                 .ToListAsync();
        }

        public async Task<IActionResult> OnPostExportToExcelAsync()
        {
            var credits = await _context.Credits.ToListAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Credits");

            var headers = new[]
            {
                "ID", 
                "Клиент", 
                "Сума", 
                "Начална дата", 
                "Крайна дата",
                "Лихва", 
                "Статус", 
                "Период",
                "Обща сума", 
                "Месечна вноска", 
                "Създаден на",
                "Последна промяна"
            };
            for (int col = 0; col < headers.Length; col++)
            {
                worksheet.Cell(1, col + 1).Value = headers[col];
                worksheet.Cell(1, col + 1).Style.Font.Bold = true;
            }

            for (int i = 0; i < credits.Count; i++)
            {
                var c = credits[i];

                worksheet.Cell(i + 2, 1).Value = c.ID;
                worksheet.Cell(i + 2, 2).Value = c.ClientID;
                worksheet.Cell(i + 2, 3).Value = c.CreditAmount;
                worksheet.Cell(i + 2, 4).Value = c.CreditBeginDate?.ToString("yyyy-MM-dd");
                worksheet.Cell(i + 2, 5).Value = c.CreditEndDate?.ToString("yyyy-MM-dd");
                worksheet.Cell(i + 2, 6).Value = c.InterestRate;
                worksheet.Cell(i + 2, 7).Value = c.Status;
                worksheet.Cell(i + 2, 8).Value = c.CreditPeriod;
                worksheet.Cell(i + 2, 9).Value = c.TotalCreditAmount;
                worksheet.Cell(i + 2, 10).Value = c.MonthlyInstallment;
                worksheet.Cell(i + 2, 11).Value = 
                    c.CreatedOn != default(DateTime)
                        ? c.CreatedOn?.ToString("yyyy-MM-dd HH:mm:ss")
                        : string.Empty;
                worksheet.Cell(i + 2, 12).Value =
                    c.ModifiedOn != default(DateTime)
                        ? c.ModifiedOn?.ToString("yyyy-MM-dd HH:mm:ss")
                        : string.Empty;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            var fileName = $"Credits_{DateTime.Now:yyyyMMdd_HHmmss}_21180011.xlsx";
            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }
    }
}
