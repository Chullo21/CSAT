using Microsoft.AspNetCore.Mvc;
using PIMES_DMS.Data;
using PIMES_DMS.Models;

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

            return View("Login_View");
        }

        public IActionResult LoginAcc(string user, string pass)
        {
            if (user == null || pass == null)
            {
                return RedirectToAction("Login");
            }

            var log = _Db.AccountsDb.FirstOrDefault(j => j.UserName == user && j.Password == pass);

            if (log != null)
            {
                TempData["EN"] = log.AccName as string;
                TempData["Role"] = log.Role as string;

                return RedirectToAction("Home", "Home");
            }
            
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}
