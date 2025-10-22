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
            try
            {
                // Log incoming data for debugging
                Console.WriteLine($"KlantId: {bestelling.KlantId}");
                Console.WriteLine($"Items count: {items?.Count ?? 0}");

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        Console.WriteLine($"Item - ProductId: {item.ProductId}, Aantal: {item.Aantal}");
                    }
                }

                // Check if items list is null or empty
                if (items == null || items.Count == 0)
                {
                    ModelState.AddModelError("", "Er zijn geen producten toegevoegd aan de bestelling.");
                }

                if (ModelState.IsValid)
                {
                    bestelling.BestelNummer = GenereerBestelNummer();
                    bestelling.BestelDatum = DateTime.Now;

                    decimal subtotaal = 0;
                    decimal btwBedrag = 0;

                    // Clear existing items to avoid duplicates
                    bestelling.BestellingItems.Clear();

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
                            Console.WriteLine($"Added item: {product.Naam}, Aantal: {item.Aantal}, Totaal: {item.Totaal}");
                        }
                    }

                    if (bestelling.BestellingItems.Count == 0)
                    {
                        TempData["Error"] = "Geen geldige producten gevonden in de bestelling.";
                        ViewData["KlantId"] = new SelectList(_context.Klanten, "Id", "Bedrijfsnaam", bestelling.KlantId);
                        var producten = await _context.Producten
                            .Where(p => p.IsActief)
                            .Select(p => new { p.Id, p.Naam, p.Prijs, p.BTWPercentage })
                            .ToListAsync();
                        ViewBag.ProductenData = producten;
                        return View(bestelling);
                    }

                    bestelling.SubTotaal = subtotaal;
                    bestelling.BTWBedrag = btwBedrag;
                    bestelling.Totaal = subtotaal + btwBedrag;

                    Console.WriteLine($"Bestelling totals - Subtotaal: {subtotaal}, BTW: {btwBedrag}, Totaal: {bestelling.Totaal}");

                    _context.Add(bestelling);
                    await _context.SaveChangesAsync();

                    Console.WriteLine($"Bestelling saved with ID: {bestelling.Id}");

                    // Genereer automatisch factuur
                    try
                    {
                        await _facturatieService.GenereerFactuur(bestelling.Id);
                        Console.WriteLine("Factuur generated successfully");
                        TempData["Success"] = "Bestelling succesvol aangemaakt en factuur gegenereerd!";
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error generating factuur: {ex.Message}");
                        TempData["Warning"] = "Bestelling aangemaakt, maar er is een fout opgetreden bij het genereren van de factuur.";
                    }

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Log validation errors
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            Console.WriteLine($"Validation error: {error.ErrorMessage}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in Create: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                TempData["Error"] = $"Er is een fout opgetreden: {ex.Message}";
            }

            // ⭐ BELANGRIJK: Voeg deze regels toe voor wanneer ModelState NIET valid is
            ViewData["KlantId"] = new SelectList(_context.Klanten, "Id", "Bedrijfsnaam", bestelling.KlantId);

            // Haal producten op met alle benodigde data (net zoals in GET)
            var productenList = await _context.Producten
                .Where(p => p.IsActief)
                .Select(p => new
                {
                    p.Id,
                    p.Naam,
                    p.Prijs,
                    p.BTWPercentage
                })
                .ToListAsync();

            ViewBag.ProductenData = productenList;

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
