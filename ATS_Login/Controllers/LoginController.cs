using ATS_Login.Data;
using ATS_Login.Models;
using Microsoft.AspNetCore.Mvc;

namespace ATS_Login.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext Db;
        public LoginController(AppDbContext Db)
        {
            this.Db = Db;
        }

        public async Task<IActionResult> Login()
        {
            await ClearMySession();

            return View("Login", "Login");
        }

        public async Task<IActionResult> TryLogin (string username, string password)
        {
            AccountsModel? accountData = Db.Accounts.FirstOrDefault(j => j.Username == username && j.Password == password);
            char returnCode = 'c';

            if (accountData != null)
            {
                ProcessSessionData(accountData);

                return RedirectToAction("AuditMenu", "Home", new { area = "QA_AUDIT" });

            }           

            return Json(new { response = returnCode });
        }

        private async Task ClearMySession()
        {
            HttpContext.Session.Clear();
        }

        private async Task ProcessSessionData(AccountsModel accountData)
        {
            string uniqueKey = Guid.NewGuid().ToString();

            HttpContext.Session.SetString("Session", uniqueKey);

            Db.Sessions.Add(new SessionModel().CreateSession(accountData.AccountName, uniqueKey));
            //Db.SaveChanges();
        }
    }
}
