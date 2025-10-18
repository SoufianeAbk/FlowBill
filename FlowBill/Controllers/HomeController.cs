using System.Diagnostics;
using FlowBill.Data;
using FlowBill.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlowBill.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dashboard = new
            {
                TotaalKlanten = await _context.Klanten.CountAsync(),
                TotaalBestellingen = await _context.Bestellingen.CountAsync(),
                OpenstaandeFacturen = await _context.Facturen.Where(f => f.Status == FactuurStatus.Openstaand).CountAsync(),
                TotaalOmzet = await _context.Facturen.Where(f => f.Status == FactuurStatus.Betaald).SumAsync(f => (decimal?)f.Totaal) ?? 0
            };

            ViewBag.Dashboard = dashboard;

            // Laatste 5 bestellingen
            ViewBag.RecenteBestellingen = await _context.Bestellingen
                .Include(b => b.Klant)
                .OrderByDescending(b => b.BestelDatum)
                .Take(5)
                .ToListAsync();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}