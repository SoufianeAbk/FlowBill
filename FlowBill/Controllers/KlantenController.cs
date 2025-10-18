using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlowBill.Data;
using FlowBill.Models;

namespace FlowBill.Controllers
{
    [Authorize]
    public class KlantenController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KlantenController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Klanten
        public async Task<IActionResult> Index()
        {
            return View(await _context.Klanten.ToListAsync());
        }

        // GET: Klanten/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klant = await _context.Klanten
                .Include(k => k.Bestellingen)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (klant == null)
            {
                return NotFound();
            }

            return View(klant);
        }

        // GET: Klanten/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Klanten/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Bedrijfsnaam,Contactpersoon,Email,Telefoon,Adres,Postcode,Stad,BTWNummer,KVKNummer")] Klant klant)
        {
            if (ModelState.IsValid)
            {
                klant.AangemaaktOp = DateTime.Now;
                _context.Add(klant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(klant);
        }

        // GET: Klanten/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klant = await _context.Klanten.FindAsync(id);
            if (klant == null)
            {
                return NotFound();
            }
            return View(klant);
        }

        // POST: Klanten/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Bedrijfsnaam,Contactpersoon,Email,Telefoon,Adres,Postcode,Stad,BTWNummer,KVKNummer,AangemaaktOp")] Klant klant)
        {
            if (id != klant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(klant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KlantExists(klant.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(klant);
        }

        // GET: Klanten/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klant = await _context.Klanten
                .FirstOrDefaultAsync(m => m.Id == id);
            if (klant == null)
            {
                return NotFound();
            }

            return View(klant);
        }

        // POST: Klanten/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var klant = await _context.Klanten.FindAsync(id);
            if (klant != null)
            {
                _context.Klanten.Remove(klant);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool KlantExists(int id)
        {
            return _context.Klanten.Any(e => e.Id == id);
        }
    }
}