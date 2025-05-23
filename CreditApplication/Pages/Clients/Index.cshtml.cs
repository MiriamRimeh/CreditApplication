using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CreditApplication.Data;
using CreditApplication.Models;
using System.Drawing.Printing;
using Microsoft.Data.SqlClient;
using ClosedXML.Excel;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace CreditApplication.Pages.Clients
{
    public class IndexModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public IndexModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }

        private const int PageSize = 20;

        public IList<Client> Client { get; set; } = new List<Client>();

        [BindProperty(SupportsGet = true)]
        public int? SearchClientId { get; set; }


        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }


        [BindProperty(SupportsGet = true)]
        public string SearchEGN { get; set; }


        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; }
        public string ClientIdSort { get; set; }
        public string NameSort { get; set; }


        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; private set; }


        public async Task OnGetAsync()
        {
            ClientIdSort = String.IsNullOrEmpty(SortOrder) ? "id_desc" : "";
            NameSort = SortOrder == "Name" ? "name_desc" : "Name";

            var query = _context.Clients.AsQueryable();

            if (SearchClientId.HasValue)
            {
                query = query.Where(c => c.ID == SearchClientId.Value);
            }

            if (!string.IsNullOrEmpty(SearchName))
            {
                query = query.Where(c => c.FirstName.Contains(SearchName) || c.LastName.Contains(SearchName));
            }

            if (!string.IsNullOrEmpty(SearchEGN))
            {
                query = query.Where(c => c.EGN.Contains(SearchEGN));
            }


            switch (SortOrder)
            {
                case "id_desc":
                    query = query.OrderByDescending(c => c.ID);
                    break;
                case "Name":
                    query = query.OrderBy(c => c.FirstName).ThenBy(c => c.LastName);
                    break;
                case "name_desc":
                    query = query.OrderByDescending(c => c.FirstName).ThenByDescending(c => c.LastName);
                    break;
                default:
                    query = query.OrderBy(c => c.ID);
                    break;
            }

            var count = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            Client = await query
                 .Skip((PageIndex - 1) * PageSize)
                 .Take(PageSize)
                 .ToListAsync();


        }

        public async Task<IActionResult> OnPostExportToExcelAsync()
        {
            var clients = await _context.Clients.ToListAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Clients");

            // Заглавия
            var headers = new[]
            {
                "ID", "Име", "Бащино име", "Фамилия", "ЕГН",
                "Имейл", "Телефон", "Номер на лична карта",
                "Дата на издаване", "Дата на валидност", "Издател (МВР)",
                "Създаден на", "Последна промяна"
            };
            for (int col = 0; col < headers.Length; col++)
            {
                worksheet.Cell(1, col + 1).Value = headers[col];
                worksheet.Cell(1, col + 1).Style.Font.Bold = true;
            }

            for (int i = 0; i < clients.Count; i++)
            {
                var c = clients[i];

                worksheet.Cell(i + 2, 1).Value = c.ID;
                worksheet.Cell(i + 2, 2).Value = c.FirstName ?? string.Empty;
                worksheet.Cell(i + 2, 3).Value = c.MiddleName ?? string.Empty;
                worksheet.Cell(i + 2, 4).Value = c.LastName ?? string.Empty;
                worksheet.Cell(i + 2, 5).Value = c.EGN ?? string.Empty;
                worksheet.Cell(i + 2, 6).Value = c.Email ?? string.Empty;
                worksheet.Cell(i + 2, 7).Value = c.PhoneNumber ?? string.Empty;
                worksheet.Cell(i + 2, 8).Value = c.IDCardNumber ?? string.Empty;

                // Не-Nullable DateTime — винаги работи
                worksheet.Cell(i + 2, 9).Value = c.IDIssueDate.ToString("yyyy-MM-dd");
                worksheet.Cell(i + 2, 10).Value = c.IDValidityDate.ToString("yyyy-MM-dd");
                worksheet.Cell(i + 2, 11).Value = c.IDIssuer ?? string.Empty;
                worksheet.Cell(i + 2, 12).Value = c.CreatedOn != default(DateTime)
                        ? c.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss")
                        : string.Empty;

                // Ако искате празно при „нулева“ дата:
                worksheet.Cell(i + 2, 13).Value =
                    c.ModifiedOn != default(DateTime)
                        ? c.ModifiedOn.ToString("yyyy-MM-dd HH:mm:ss")
                        : string.Empty;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            var fileName = $"Clients_{DateTime.Now:yyyyMMdd_HHmmss}_21180011.xlsx";
            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }
    }
}
