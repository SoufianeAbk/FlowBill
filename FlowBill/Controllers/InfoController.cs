using Microsoft.AspNetCore.Mvc;

namespace FlowBill.Controllers
{
    public class InfoController : Controller
    {
        // GET: Info/FAQ
        public IActionResult FAQ()
        {
            return View();
        }

        // GET: Info/About
        public IActionResult About()
        {
            return View();
        }

        // GET: Info/Delivery
        public IActionResult Delivery()
        {
            return View();
        }
    }
}