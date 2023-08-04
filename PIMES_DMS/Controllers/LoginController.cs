using Microsoft.AspNetCore.Mvc;
using PIMES_DMS.Data;


namespace PIMES_DMS.Controllers
{
    public class LoginController : Controller
    {

        private readonly AppDbContext _Db;

        public LoginController(AppDbContext db)
        {
            _Db = db;
        }

        public IActionResult Login()
        {
            TempData.Clear();
            TempData["loginTimes"] = 0;

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

                return View("Login_View");
            }

            var log = _Db.AccountsDb.FirstOrDefault(j => j.UserName == user && j.Password == pass);

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

                return View("Login_View");
            }
        }


        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}
