using Microsoft.AspNetCore.Mvc;
using PIMES_DMS.Data;
using PIMES_DMS.Models;

namespace PIMES_DMS.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _Db;
        private readonly List<AccountsModel> mainAccounts = new List<AccountsModel>();
        private readonly List<AnnouncementModel>? mainAnns = new List<AnnouncementModel>();

        public LoginController(AppDbContext db)
        {
            _Db = db;
            mainAccounts = _Db.AccountsDb.ToList();
            mainAnns = _Db.AnnDb.ToList();
        }

        private void GetAnnouncements()
        {
            ViewBag.Announcements = mainAnns.Where(j => j.Type == "Everyone").ToList();
        }

        public IActionResult Login()
        {
            GetAnnouncements();

            return View("Login_View");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult LoginAcc(string user, string pass)
        {
            int loginTimes = (int)TempData["loginTimes"]!;

            if (user == null || pass == null || (user == null && pass == null))
            {
                loginTimes += 1;
                TempData["loginTimes"] = loginTimes;

                TempData["message"] = "Please input your log-in credentials.";

                return RedirectToAction("Login");
            }

            var log = mainAccounts.FirstOrDefault(j => j.UserName == user && j.Password == pass);

            if (log != null)
            {
                TempData["EN"] = log.AccName as string;
                TempData["Role"] = log.Role as string;

                return RedirectToAction("DashView", "Dashboard");
            }
            else
            {
                loginTimes += 1;
                TempData["loginTimes"] = loginTimes;
                TempData["message"] = "Invalid log-in credentials.";

                return RedirectToAction("Login");
            }
        }

        public IActionResult Logout()
        {
            TempData.Clear();
            TempData["loginTimes"] = 0;

            return RedirectToAction("Login");
        }
    }
}
