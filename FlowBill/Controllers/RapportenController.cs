using ClosedXML.Excel;
using FlowBill.Data;
using FlowBill.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Text;

namespace FlowBill.Controllers
{
    [Authorize]
    public class RapportenController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RapportenController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rapporten
        public async Task<IActionResult> Index()
        {
            var viewModel = new RapportViewModel
            {
                // Huidige maand statistieken
                OmzetHuidigeMaand = await BerekenOmzet(DateTime.Now.Month, DateTime.Now.Year),
                AantalFacturenHuidigeMaand = await _context.Facturen
                    .Where(f => f.FactuurDatum.Month == DateTime.Now.Month && f.FactuurDatum.Year == DateTime.Now.Year)
                    .CountAsync(),

                // Vorige maand statistieken
                OmzetVorigeMaand = await BerekenOmzet(DateTime.Now.AddMonths(-1).Month, DateTime.Now.AddMonths(-1).Year),

                // Jaar statistieken
                OmzetDitJaar = await _context.Facturen
                    .Where(f => f.FactuurDatum.Year == DateTime.Now.Year && f.Status == FactuurStatus.Betaald)
                    .SumAsync(f => (decimal?)f.Totaal) ?? 0,

                // Openstaande facturen
                OpenstaandeFacturen = await _context.Facturen
                    .Where(f => f.Status == FactuurStatus.Openstaand)
                    .Include(f => f.Bestelling)
                        .ThenInclude(b => b.Klant)
                    .OrderBy(f => f.VervalDatum)
                    .Take(10)
                    .ToListAsync(),

                // Top klanten
                TopKlanten = await _context.Klanten
                    .Select(k => new TopKlantViewModel
                    {
                        Klant = k,
                        TotaalBedrag = k.Bestellingen.Sum(b => (decimal?)b.Totaal) ?? 0,
                        AantalBestellingen = k.Bestellingen.Count()
                    })
                    .OrderByDescending(tk => tk.TotaalBedrag)
                    .Take(5)
                    .ToListAsync(),

                // Top producten
                TopProducten = await _context.BestellingItems
                    .GroupBy(bi => bi.Product)
                    .Select(g => new TopProductViewModel
                    {
                        Product = g.Key,
                        AantalVerkocht = g.Sum(bi => bi.Aantal),
                        TotaalOmzet = g.Sum(bi => bi.Totaal)
                    })
                    .OrderByDescending(tp => tp.TotaalOmzet)
                    .Take(5)
                    .ToListAsync(),

                // Maandelijkse omzet voor grafiek
                MaandelijkseOmzet = await GetMaandelijkseOmzet()
            };

            return View(viewModel);
        }

        // GET: Rapporten/ExportFacturen
        public async Task<IActionResult> ExportFacturen(DateTime? startDatum, DateTime? eindDatum)
        {
            startDatum ??= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            eindDatum ??= DateTime.Now;

            var facturen = await _context.Facturen
                .Include(f => f.Bestelling)
                    .ThenInclude(b => b.Klant)
                .Where(f => f.FactuurDatum >= startDatum && f.FactuurDatum <= eindDatum)
                .OrderBy(f => f.FactuurDatum)
                .ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Facturen");
                var currentRow = 1;

                // Headers
                worksheet.Cell(currentRow, 1).Value = "Factuurnummer";
                worksheet.Cell(currentRow, 2).Value = "Datum";
                worksheet.Cell(currentRow, 3).Value = "Klant";
                worksheet.Cell(currentRow, 4).Value = "Subtotaal";
                worksheet.Cell(currentRow, 5).Value = "BTW";
                worksheet.Cell(currentRow, 6).Value = "Totaal";
                worksheet.Cell(currentRow, 7).Value = "Status";
                worksheet.Cell(currentRow, 8).Value = "Betaald";

                // Style headers
                var headerRange = worksheet.Range(1, 1, 1, 8);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

                // Data
                foreach (var factuur in facturen)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = factuur.FactuurNummer;
                    worksheet.Cell(currentRow, 2).Value = factuur.FactuurDatum.ToString("dd-MM-yyyy");
                    worksheet.Cell(currentRow, 3).Value = factuur.Bestelling.Klant.Bedrijfsnaam;
                    worksheet.Cell(currentRow, 4).Value = factuur.SubTotaal;
                    worksheet.Cell(currentRow, 5).Value = factuur.BTWBedrag;
                    worksheet.Cell(currentRow, 6).Value = factuur.Totaal;
                    worksheet.Cell(currentRow, 7).Value = factuur.Status.ToString();
                    worksheet.Cell(currentRow, 8).Value = factuur.Status == FactuurStatus.Betaald ? "Ja" : "Nee";
                }

                // Format columns
                worksheet.Column(4).Style.NumberFormat.Format = "€ #,##0.00";
                worksheet.Column(5).Style.NumberFormat.Format = "€ #,##0.00";
                worksheet.Column(6).Style.NumberFormat.Format = "€ #,##0.00";

                // Auto fit columns
                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    var fileName = $"Facturen_{startDatum:yyyyMMdd}_{eindDatum:yyyyMMdd}.xlsx";

                    return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
                }
            }
        }

        // GET: Rapporten/OmzetRapport
        public async Task<IActionResult> OmzetRapport(int? jaar)
        {
            jaar ??= DateTime.Now.Year;

            var omzetPerMaand = new Dictionary<int, decimal>();
            for (int maand = 1; maand <= 12; maand++)
            {
                omzetPerMaand[maand] = await BerekenOmzet(maand, jaar.Value);
            }

            ViewBag.Jaar = jaar;
            ViewBag.Jaren = Enumerable.Range(2020, DateTime.Now.Year - 2019)
                .Select(y => new SelectListItem { Value = y.ToString(), Text = y.ToString() });

            return View(omzetPerMaand);
        }

        private async Task<decimal> BerekenOmzet(int maand, int jaar)
        {
            return await _context.Facturen
                .Where(f => f.FactuurDatum.Month == maand &&
                           f.FactuurDatum.Year == jaar &&
                           f.Status == FactuurStatus.Betaald)
                .SumAsync(f => (decimal?)f.Totaal) ?? 0;
        }

        private async Task<Dictionary<string, decimal>> GetMaandelijkseOmzet()
        {
            var result = new Dictionary<string, decimal>();
            var vandaag = DateTime.Now;

            for (int i = 11; i >= 0; i--)
            {
                var datum = vandaag.AddMonths(-i);
                var maandNaam = datum.ToString("MMM yyyy");
                result[maandNaam] = await BerekenOmzet(datum.Month, datum.Year);
            }

            return result;
        }
    }

    // ViewModels
    public class RapportViewModel
    {
        public decimal OmzetHuidigeMaand { get; set; }
        public int AantalFacturenHuidigeMaand { get; set; }
        public decimal OmzetVorigeMaand { get; set; }
        public decimal OmzetDitJaar { get; set; }
        public List<Factuur> OpenstaandeFacturen { get; set; }
        public List<TopKlantViewModel> TopKlanten { get; set; }
        public List<TopProductViewModel> TopProducten { get; set; }
        public Dictionary<string, decimal> MaandelijkseOmzet { get; set; }
    }

    public class TopKlantViewModel
    {
        public Klant Klant { get; set; }
        public decimal TotaalBedrag { get; set; }
        public int AantalBestellingen { get; set; }
    }

    public class TopProductViewModel
    {
        public Product Product { get; set; }
        public int AantalVerkocht { get; set; }
        public decimal TotaalOmzet { get; set; }
    }
}