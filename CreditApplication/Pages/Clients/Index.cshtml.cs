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

        public IList<Client> Client { get; set; }

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

            //Client = await _context.Clients
            //   // .Include(c => c.ClientAddress)
            //    .ToListAsync();
        }

        public async Task<IActionResult> OnPostExportToExcelAsync()
        {
            var clients = await _context.Clients.ToListAsync();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Clients");
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Име";
                worksheet.Cell(1, 3).Value = "Бащино име";
                worksheet.Cell(1, 4).Value = "Фамилия";
                worksheet.Cell(1, 5).Value = "ЕГН";
                worksheet.Cell(1, 6).Value = "Имейл";
                worksheet.Cell(1, 7).Value = "Телефон";
                worksheet.Cell(1, 8).Value = "Номер на лична карта";
                worksheet.Cell(1, 9).Value = "Дата на издаване";
                worksheet.Cell(1, 10).Value = "Дата на валидност";
                worksheet.Cell(1, 11).Value = "Издател (МВР)";
                worksheet.Cell(1, 12).Value = "Създаден на";
                worksheet.Cell(1, 13).Value = "Последна промяна";
                for (int i = 0; i < clients.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = clients[i].ID;
                    worksheet.Cell(i + 2, 2).Value = clients[i].FirstName;
                    worksheet.Cell(i + 2, 3).Value = clients[i].MiddleName ?? string.Empty;
                    worksheet.Cell(i + 2, 4).Value = clients[i].LastName;
                    worksheet.Cell(i + 2, 5).Value = clients[i].EGN;
                    worksheet.Cell(i + 2, 6).Value = clients[i].Email ?? string.Empty;
                    worksheet.Cell(i + 2, 7).Value = clients[i].PhoneNumber;
                    worksheet.Cell(i + 2, 8).Value = clients[i].IDCardNumber;
                    worksheet.Cell(i + 2, 9).Value = clients[i].IDIssueDate.ToString("yyyy-MM-dd");
                    worksheet.Cell(i + 2, 10).Value = clients[i].IDValidityDate.ToString("yyyy-MM-dd");
                    worksheet.Cell(i + 2, 11).Value = clients[i].IDIssuer;
                    worksheet.Cell(i + 2, 12).Value = clients[i].CreatedOn;
                    worksheet.Cell(i + 2, 13).Value = clients[i].ModifiedOn;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var fileName = $"Clients_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}
