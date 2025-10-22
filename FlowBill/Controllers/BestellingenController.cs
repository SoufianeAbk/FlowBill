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
        private readonly ILogger<BestellingenController> _logger;

        public BestellingenController(
            ApplicationDbContext context,
            IFacturatieService facturatieService,
            ILogger<BestellingenController> logger)
        {
            _context = context;
            _facturatieService = facturatieService;
            _logger = logger;
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
                // Enhanced logging for debugging
                _logger.LogInformation("=== CREATE BESTELLING CALLED ===");
                _logger.LogInformation($"KlantId: {bestelling.KlantId}");
                _logger.LogInformation($"Items received: {items?.Count ?? 0}");

                if (items != null && items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        _logger.LogInformation($"Item - ProductId: {item.ProductId}, Aantal: {item.Aantal}");
                    }
                }
                else
                {
                    _logger.LogWarning("No items received in POST request!");
                }

                // Remove BestellingId validation errors since it's not sent from the form
                ModelState.Remove("BestellingId");
                foreach (var i in Enumerable.Range(0, items?.Count ?? 0))
                {
                    ModelState.Remove($"items[{i}].BestellingId");
                    ModelState.Remove($"items[{i}].Id");
                    ModelState.Remove($"items[{i}].PrijsPerStuk");
                    ModelState.Remove($"items[{i}].BTWPercentage");
                    ModelState.Remove($"items[{i}].Totaal");
                }

                // Check if items list is null or empty
                if (items == null || items.Count == 0)
                {
                    _logger.LogError("No items in the order!");
                    ModelState.AddModelError("", "Er zijn geen producten toegevoegd aan de bestelling.");
                    TempData["Error"] = "Voeg minimaal één product toe aan de bestelling.";
                }

                // Log ModelState errors
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("ModelState is invalid:");
                    foreach (var modelState in ModelState)
                    {
                        foreach (var error in modelState.Value.Errors)
                        {
                            _logger.LogWarning($"  Key: {modelState.Key}, Error: {error.ErrorMessage}");
                        }
                    }
                }

                if (ModelState.IsValid && items != null && items.Count > 0)
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
                            _logger.LogInformation($"Added item: {product.Naam}, Aantal: {item.Aantal}, Totaal: {item.Totaal}");
                        }
                        else
                        {
                            _logger.LogWarning($"Product with ID {item.ProductId} not found!");
                        }
                    }

                    if (bestelling.BestellingItems.Count == 0)
                    {
                        _logger.LogError("No valid items after processing!");
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

                    _logger.LogInformation($"Bestelling totals - Subtotaal: {subtotaal}, BTW: {btwBedrag}, Totaal: {bestelling.Totaal}");

                    _context.Add(bestelling);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"✅ Bestelling saved successfully with ID: {bestelling.Id}");

                    // Genereer automatisch factuur
                    try
                    {
                        _logger.LogInformation("Attempting to generate factuur...");
                        await _facturatieService.GenereerFactuur(bestelling.Id);
                        _logger.LogInformation("✅ Factuur generated successfully");
                        TempData["Success"] = $"Bestelling {bestelling.BestelNummer} succesvol aangemaakt en factuur gegenereerd!";
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "❌ Error generating factuur");
                        TempData["Warning"] = $"Bestelling {bestelling.BestelNummer} aangemaakt, maar er is een fout opgetreden bij het genereren van de factuur: {ex.Message}";
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception in Create method");
                TempData["Error"] = $"Er is een fout opgetreden: {ex.Message}";
            }

            // If we got here, something failed - reload the form
            ViewData["KlantId"] = new SelectList(_context.Klanten, "Id", "Bedrijfsnaam", bestelling.KlantId);

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
            try
            {
                var factuur = await _facturatieService.GenereerFactuur(id);
                if (factuur != null)
                {
                    TempData["Success"] = "Factuur succesvol gegenereerd en verzonden!";
                    return RedirectToAction("Details", "Facturen", new { id = factuur.Id });
                }

                TempData["Error"] = "Er is een fout opgetreden bij het genereren van de factuur.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error generating factuur for bestelling {id}");
                TempData["Error"] = $"Er is een fout opgetreden: {ex.Message}";
            }

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