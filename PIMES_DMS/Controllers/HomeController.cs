using Microsoft.AspNetCore.Mvc;
using PIMES_DMS.Data;
using PIMES_DMS.Models;
using System.Diagnostics;

namespace PIMES_DMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _Db;

        public HomeController(AppDbContext db)
        {
            _Db = db;
        }

        public IActionResult Home()
        {
            return View();
        }

        public IActionResult AdminHome()
        {

            ViewData["InComingIssue"] = _Db.IssueDb.Where(j => !j.Acknowledged && !j.ValidatedStatus);
            ViewData["OnProgressIssue"] = _Db.IssueDb.Where(j => j.Acknowledged && !j.ValidatedStatus);

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