using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlowBill.Data;
using FlowBill.Models;
using FlowBill.Services;

namespace FlowBill.Controllers
{
    [Authorize]
    public class FacturenController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFacturatieService _facturatieService;

        public FacturenController(ApplicationDbContext context, IFacturatieService facturatieService)
        {
            _context = context;
            _facturatieService = facturatieService;
        }

        // GET: Facturen
        public async Task<IActionResult> Index()
        {
            var facturen = await _context.Facturen
                .Include(f => f.Bestelling)
                    .ThenInclude(b => b.Klant)
                .OrderByDescending(f => f.FactuurDatum)
                .ToListAsync();
            return View(facturen);
        }

        // GET: Facturen/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factuur = await _context.Facturen
                .Include(f => f.Bestelling)
                    .ThenInclude(b => b.Klant)
                .Include(f => f.Bestelling)
                    .ThenInclude(b => b.BestellingItems)
                        .ThenInclude(bi => bi.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (factuur == null)
            {
                return NotFound();
            }

            return View(factuur);
        }

        // GET: Facturen/Download/5
        public async Task<IActionResult> Download(int id)
        {
            var factuur = await _context.Facturen
                .Include(f => f.Bestelling)
                    .ThenInclude(b => b.Klant)
                .Include(f => f.Bestelling)
                    .ThenInclude(b => b.BestellingItems)
                        .ThenInclude(bi => bi.Product)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (factuur == null)
            {
                return NotFound();
            }

            var pdfBytes = await _facturatieService.GenereerPDF(factuur);
            return File(pdfBytes, "application/pdf", $"{factuur.FactuurNummer}.pdf");
        }

        // POST: Facturen/MarkeerBetaald/5
        [HttpPost]
        public async Task<IActionResult> MarkeerBetaald(int id)
        {
            var factuur = await _context.Facturen.FindAsync(id);
            if (factuur == null)
            {
                return NotFound();
            }

            factuur.Status = FactuurStatus.Betaald;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Factuur gemarkeerd als betaald.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Facturen/VerstuurOpnieuw/5
        public async Task<IActionResult> VerstuurOpnieuw(int id)
        {
            var factuur = await _context.Facturen.FindAsync(id);
            if (factuur == null)
            {
                return NotFound();
            }

            var result = await _facturatieService.VerstuurFactuurEmail(factuur);
            if (result)
            {
                TempData["Success"] = "Factuur succesvol opnieuw verzonden.";
            }
            else
            {
                TempData["Error"] = "Er is een fout opgetreden bij het verzenden van de factuur.";
            }

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}