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

namespace CreditApplication.Pages.ClientAddresses
{
    public class IndexModel : PageModel
    {
        private readonly CreditApplication.Data.CreditApplicationDbContext _context;

        public IndexModel(CreditApplication.Data.CreditApplicationDbContext context)
        {
            _context = context;
        }
        private const int PageSize = 20;

        public IList<ClientAddress> ClientAddress { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public int? SearchClientId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchEGN { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchCity { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchPostalCode { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; }
        public string ClientIdSort { get; set; }
        public string CitySort { get; set; }
        public string PostalCodeSort { get; set; }
        public string StreetSort { get; set; }


        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; private set; }
        public async Task OnGetAsync()
        {
            ClientIdSort = String.IsNullOrEmpty(SortOrder) ? "clientid_desc" : "";
            CitySort = SortOrder == "City" ? "city_desc" : "City";
            PostalCodeSort = SortOrder == "PostalCode" ? "postal_desc" : "PostalCode";
            StreetSort = SortOrder == "Street" ? "street_desc" : "Street";

            var query = _context.ClientAddresses
                .Include(c => c.Client)
                .AsQueryable();

            if (SearchClientId.HasValue)
                query = query.Where(ca => ca.ClientID == SearchClientId.Value);

            if (!string.IsNullOrWhiteSpace(SearchEGN))
                query = query.Where(c => c.Client != null
                                      && EF.Functions.Like(c.Client.EGN, $"%{SearchEGN}%"));

            if (!string.IsNullOrEmpty(SearchCity))
                query = query.Where(ca => EF.Functions.Like(ca.City, $"%{SearchCity}%"));

            if (!string.IsNullOrEmpty(SearchPostalCode))
                query = query.Where(ca => EF.Functions.Like(ca.PostCode, $"%{SearchPostalCode}%"));

            switch (SortOrder)
            {
                case "clientid_desc":
                    query = query.OrderByDescending(ca => ca.ClientID);
                    break;
                case "City":
                    query = query.OrderBy(ca => ca.City);
                    break;
                case "city_desc":
                    query = query.OrderByDescending(ca => ca.City);
                    break;
                case "PostalCode":
                    query = query.OrderBy(ca => ca.PostCode);
                    break;
                case "postal_desc":
                    query = query.OrderByDescending(ca => ca.PostCode);
                    break;
                case "Street":
                    query = query.OrderBy(ca => ca.StreetNeighbourhood);
                    break;
                case "street_desc":
                    query = query.OrderByDescending(ca => ca.StreetNeighbourhood);
                    break;
                default:
                    query = query.OrderBy(ca => ca.ClientID);
                    break;
            }

            var count = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            ClientAddress = await query
                 .Skip((PageIndex - 1) * PageSize)
                 .Take(PageSize)
                 .ToListAsync();
        }

        public async Task<IActionResult> OnPostExportToExcelAsync()
        {
            var data = await _context.ClientAddresses
                .Include(c => c.Client)
                .ToListAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("ClientAddresses");
            var headers = new[] { "№ на клиент", "Град", "Улица/Квартал","Номер", "Пощенски код", "Създадено на", "Последна промяна" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
            }

            for (int i = 0; i < data.Count; i++)
            {
                var ca = data[i];
                var row = i + 2;
                worksheet.Cell(row, 1).Value = ca.ClientID;
                worksheet.Cell(row, 2).Value = ca.City;
                worksheet.Cell(row, 3).Value = ca.StreetNeighbourhood;
                worksheet.Cell(row, 4).Value = ca.Number;
                worksheet.Cell(row, 5).Value = ca.PostCode;
                worksheet.Cell(row, 6).Value = ca.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss");
                worksheet.Cell(row, 7).Value = ca.ModifiedOn.ToString("yyyy-MM-dd HH:mm:ss");
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            var fileName = $"ClientAddresses_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }
    }
}
