using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlowBill.Data;
using FlowBill.Models;

namespace FlowBill.Controllers
{
    [Authorize]
    public class ProductenController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductenController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Producten
        public async Task<IActionResult> Index(int? categoryId)
        {
            var producten = _context.Producten.Include(p => p.Category).AsQueryable();

            if (categoryId.HasValue)
            {
                producten = producten.Where(p => p.CategoryId == categoryId);
            }

            ViewBag.Categories = await _context.ProductCategories.ToListAsync();
            ViewBag.SelectedCategoryId = categoryId;

            return View(await producten.ToListAsync());
        }

        // GET: Producten/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Producten
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Producten/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await _context.ProductCategories.ToListAsync(), "Id", "Naam");
            return View();
        }

        // POST: Producten/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Naam,CategoryId,Omschrijving,Prijs,BTWPercentage,Voorraad,SKU,IsActief")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.AangemaaktOp = DateTime.Now;
                _context.Add(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Product succesvol aangemaakt!";
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(await _context.ProductCategories.ToListAsync(), "Id", "Naam", product.CategoryId);
            return View(product);
        }

        // GET: Producten/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Producten.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(await _context.ProductCategories.ToListAsync(), "Id", "Naam", product.CategoryId);
            return View(product);
        }

        // POST: Producten/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naam,CategoryId,Omschrijving,Prijs,BTWPercentage,Voorraad,SKU,IsActief,AangemaaktOp")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Product succesvol bijgewerkt!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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

            ViewData["CategoryId"] = new SelectList(await _context.ProductCategories.ToListAsync(), "Id", "Naam", product.CategoryId);
            return View(product);
        }

        // GET: Producten/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Producten
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Producten/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Producten.FindAsync(id);
            if (product != null)
            {
                _context.Producten.Remove(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Product succesvol verwijderd!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Producten.Any(e => e.Id == id);
        }
    }
}