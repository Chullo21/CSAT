using Microsoft.AspNetCore.Mvc;
using QA_AUDIT.Models;
using System.Diagnostics;

namespace QA_AUDIT.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {

        }

        public IActionResult AuditMenu()
        {
            ViewData["UserData"] = HttpContext.Session.GetString("Session").ToString();
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
