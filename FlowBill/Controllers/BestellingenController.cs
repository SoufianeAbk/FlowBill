using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlowBill.Data;
using FlowBill.Models;
using FlowBill.Services;

namespace FlowBill.Controllers
{
    [Authorize]
    public class BestellingenController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFacturatieService _facturatieService;

        public BestellingenController(ApplicationDbContext context, IFacturatieService facturatieService)
        {
            _context = context;
            _facturatieService = facturatieService;
        }

        // GET: Bestellingen
        public async Task<IActionResult> Index()
        {
            var bestellingen = await _context.Bestellingen
                .Include(b => b.Klant)
                .Include(b => b.Factuur)
                .OrderByDescending(b => b.BestelDatum)
                .ToListAsync();
            return View(bestellingen);
        }

        // GET: Bestellingen/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bestelling = await _context.Bestellingen
                .Include(b => b.Klant)
                .Include(b => b.BestellingItems)
                    .ThenInclude(bi => bi.Product)
                .Include(b => b.Factuur)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (bestelling == null)
            {
                return NotFound();
            }

            return View(bestelling);
        }

        // GET: Bestellingen/Create
        public async Task<IActionResult> Create()
        {
            ViewData["KlantId"] = new SelectList(_context.Klanten, "Id", "Bedrijfsnaam");

            // Haal producten op met alle benodigde data
            var producten = await _context.Producten
                .Where(p => p.IsActief)
                .Select(p => new
                {
                    p.Id,
                    p.Naam,
                    p.Prijs,
                    p.BTWPercentage
                })
                .ToListAsync();

            ViewBag.ProductenData = producten;

            var bestelling = new Bestelling();
            bestelling.BestelNummer = GenereerBestelNummer();

            return View(bestelling);
        }

        // POST: Bestellingen/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Bestelling bestelling, List<BestellingItem> items)
        {
            if (ModelState.IsValid)
            {
                bestelling.BestelNummer = GenereerBestelNummer();
                bestelling.BestelDatum = DateTime.Now;

                decimal subtotaal = 0;
                decimal btwBedrag = 0;

                foreach (var item in items.Where(i => i.ProductId > 0 && i.Aantal > 0))
                {
                    var product = await _context.Producten.FindAsync(item.ProductId);
                    if (product != null)
                    {
                        item.PrijsPerStuk = product.Prijs;
                        item.BTWPercentage = product.BTWPercentage;
                        item.Totaal = item.Aantal * product.Prijs;

                        subtotaal += item.Totaal;
                        btwBedrag += item.Totaal * (product.BTWPercentage / 100m);

                        bestelling.BestellingItems.Add(item);
                    }
                }

                bestelling.SubTotaal = subtotaal;
                bestelling.BTWBedrag = btwBedrag;
                bestelling.Totaal = subtotaal + btwBedrag;

                _context.Add(bestelling);
                await _context.SaveChangesAsync();

                // Genereer automatisch factuur
                await _facturatieService.GenereerFactuur(bestelling.Id);

                return RedirectToAction(nameof(Index));
            }

            // ⭐ BELANGRIJK: Voeg deze regels toe voor wanneer ModelState NIET valid is
            ViewData["KlantId"] = new SelectList(_context.Klanten, "Id", "Bedrijfsnaam", bestelling.KlantId);

            // Haal producten op met alle benodigde data (net zoals in GET)
            var producten = await _context.Producten
                .Where(p => p.IsActief)
                .Select(p => new
                {
                    p.Id,
                    p.Naam,
                    p.Prijs,
                    p.BTWPercentage
                })
                .ToListAsync();

            ViewBag.ProductenData = producten; // ⭐ Dit ontbrak!

            return View(bestelling);
        }

        // GET: Bestellingen/GenereerFactuur/5
        public async Task<IActionResult> GenereerFactuur(int id)
        {
            var factuur = await _facturatieService.GenereerFactuur(id);
            if (factuur != null)
            {
                TempData["Success"] = "Factuur succesvol gegenereerd en verzonden!";
                return RedirectToAction("Details", "Facturen", new { id = factuur.Id });
            }

            TempData["Error"] = "Er is een fout opgetreden bij het genereren van de factuur.";
            return RedirectToAction(nameof(Details), new { id });
        }

        private string GenereerBestelNummer()
        {
            var jaar = DateTime.Now.Year;
            var laatste = _context.Bestellingen
                .Where(b => b.BestelNummer.StartsWith($"BST{jaar}"))
                .OrderByDescending(b => b.BestelNummer)
                .FirstOrDefault();

            int volgNummer = 1;
            if (laatste != null)
            {
                var laatsteNummer = laatste.BestelNummer.Substring(7);
                if (int.TryParse(laatsteNummer, out int nummer))
                {
                    volgNummer = nummer + 1;
                }
            }

            return $"BST{jaar}{volgNummer:D4}";
        }

        private bool BestellingExists(int id)
        {
            return _context.Bestellingen.Any(e => e.Id == id);
        }
    }
}