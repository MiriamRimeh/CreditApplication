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

namespace CreditApplication.Pages.RepaymentPlans
{
    public class IndexModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public IndexModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int? CreditId { get; set; }
        public IList<RepaymentPlan> RepaymentPlan { get;set; } = default!;

        [TempData]
        public string? StatusMessage { get; set; }
        public int PageSize = 20;

        [BindProperty(SupportsGet = true)]
        public int? CreditID { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? SearchInstallmentDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? SearchPayedOnDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; }
        public string CreditIDSort { get; set; }
        public string InstallmentNumberSort { get; set; }
        public string InstallmentDateSort { get; set; }
        public string PayedOnDateSort { get; set; }
        public string AmountSort { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; private set; }

        public async Task OnGetAsync()
        {

            CreditIDSort = String.IsNullOrEmpty(SortOrder) ? "credit_desc" : "";
            InstallmentNumberSort = SortOrder == "InstallmentNumber" ? "installment_desc" : "InstallmentNumber";
            InstallmentDateSort = SortOrder == "InstallmentDate" ? "date_desc" : "InstallmentDate";
            PayedOnDateSort = SortOrder == "PayedOnDate" ? "payedon_desc" : "PayedOnDate";
            AmountSort = SortOrder == "InstallmentAmount" ? "amount_desc" : "InstallmentAmount";

            var query = _context.RepaymentPlans
                            .Include(r => r.Credit)
                            .AsQueryable();

            if (CreditId.HasValue)
            {
                query = query.Where(rp => rp.CreditID == CreditId.Value);
            }

            if (SearchInstallmentDate.HasValue)
            {
                var d = DateOnly.FromDateTime(SearchInstallmentDate.Value);
                query = query.Where(rp => rp.InstallmentDate.HasValue
                                      && rp.InstallmentDate.Value == d);
            }

            if (SearchPayedOnDate.HasValue)
            {
                var d = SearchPayedOnDate.Value.Date;
                query = query.Where(rp => rp.PayedOnDate.HasValue
                                      && rp.PayedOnDate.Value.Date == d);
            }

            switch (SortOrder)
            {
                case "credit_desc":
                    query = query.OrderByDescending(rp => rp.CreditID);
                    break;
                case "InstallmentNumber":
                    query = query.OrderBy(rp => rp.InstallmentNumber);
                    break;
                case "installment_desc":
                    query = query.OrderByDescending(rp => rp.InstallmentNumber);
                    break;
                case "InstallmentDate":
                    query = query.OrderBy(rp => rp.InstallmentDate);
                    break;
                case "date_desc":
                    query = query.OrderByDescending(rp => rp.InstallmentDate);
                    break;
                case "PayedOnDate":
                    query = query.OrderBy(rp => rp.PayedOnDate);
                    break;
                case "payedon_desc":
                    query = query.OrderByDescending(rp => rp.PayedOnDate);
                    break;
                case "InstallmentAmount":
                    query = query.OrderBy(rp => rp.InstallmentAmount);
                    break;
                case "amount_desc":
                    query = query.OrderByDescending(rp => rp.InstallmentAmount);
                    break;
                default:
                    query = query.OrderBy(rp => rp.CreditID);
                    break;
            }

            var count = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            RepaymentPlan = await query
                .Skip((PageIndex - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

        }

        public async Task<IActionResult> OnPostPayAsync(int id)
        {
            var rp = await _context.RepaymentPlans.FindAsync(id);
            if (rp == null)
                return NotFound();

            rp.PayedOnDate = DateTime.Today;
            rp.ModifiedOn = DateTime.Now;

            var finOp = new FinancialOperation
            {
                CreditID = rp.CreditID,                         
                PayedOnDate = rp.PayedOnDate.Value,                 
                PayedAmount = rp.InstallmentAmount ?? 0m,           
                OperationType = 202,
                RepaymentPlanID = rp.ID
            };

            _context.FinancialOperations.Add(finOp);

            await _context.SaveChangesAsync();

            var remaining = await _context.RepaymentPlans.AnyAsync(p => p.CreditID == rp.CreditID && p.PayedOnDate == null);

            if (!remaining)
            {
                var credit = await _context.Credits.FindAsync(rp.CreditID);
                if (credit != null)
                {
                    credit.Status = 103;
                    credit.CreditEndDate = rp.PayedOnDate;
                    credit.ModifiedOn = DateTime.Now;
                    await _context.SaveChangesAsync();
                }
            }


            if (rp.InstallmentDate.HasValue)
            {
                DateTime installmentDate = rp.InstallmentDate.Value.ToDateTime(TimeOnly.MinValue);
                int diff = (DateTime.Today - installmentDate).Days;

                if (diff < 0)
                {
                    StatusMessage = $"Вноската е платена {-diff} дни предварително";
                }
                else if (diff > 0)
                {
                    StatusMessage = $"Вноската е платена с {diff} дни закъснение";
                }
                else
                {
                    StatusMessage = "Вноската е платена днес";
                }
            }
            else
            {
                StatusMessage = "Вноската е маркирана като платена";
            }

            return RedirectToPage(new { CreditId = this.CreditId });
        }

        public async Task<IActionResult> OnPostExportFilteredToExcelAsync()
        {
            var query = _context.RepaymentPlans
                .Include(r => r.Credit)
                .AsQueryable();

            if (CreditID.HasValue)
                query = query.Where(rp => rp.CreditID == CreditID.Value);
            if (SearchInstallmentDate.HasValue)
            {
                var d = DateOnly.FromDateTime(SearchInstallmentDate.Value);
                query = query.Where(rp => rp.InstallmentDate.HasValue && rp.InstallmentDate.Value == d);
            }
            if (SearchPayedOnDate.HasValue)
            {
                var d = SearchPayedOnDate.Value.Date;
                query = query.Where(rp => rp.PayedOnDate.HasValue && rp.PayedOnDate.Value.Date == d);
            }

            var data = await query.ToListAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("RepaymentPlans");
            var headers = new[] { "№ на кредит", "№ на вноска", "Дата на вноска", "Сума", "Дата на плащане" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
            }
            for (int i = 0; i < data.Count; i++)
            {
                var r = data[i];
                var row = i + 2;
                worksheet.Cell(row, 1).Value = r.CreditID;
                worksheet.Cell(row, 2).Value = r.InstallmentNumber;
                worksheet.Cell(row, 3).Value = r.InstallmentDate?.ToString("yyyy-MM-dd");
                worksheet.Cell(row, 4).Value = r.InstallmentAmount;
                worksheet.Cell(row, 5).Value = r.PayedOnDate?.ToString("yyyy-MM-dd");
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            var fileName = $"RepaymentPlans_Filtered_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }

        public async Task<IActionResult> OnPostExportAllToExcelAsync()
        {
            var data = await _context.RepaymentPlans
                .Include(r => r.Credit)
                .ToListAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("RepaymentPlans");
            var headers = new[] { "CreditID", "InstallmentNumber", "InstallmentDate", "InstallmentAmount", "PayedOnDate" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
            }
            for (int i = 0; i < data.Count; i++)
            {
                var r = data[i];
                var row = i + 2;
                worksheet.Cell(row, 1).Value = r.CreditID;
                worksheet.Cell(row, 2).Value = r.InstallmentNumber;
                worksheet.Cell(row, 3).Value = r.InstallmentDate?.ToString("yyyy-MM-dd");
                worksheet.Cell(row, 4).Value = r.InstallmentAmount;
                worksheet.Cell(row, 5).Value = r.PayedOnDate?.ToString("yyyy-MM-dd");
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            var fileName = $"RepaymentPlans_All_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }
    }
}
