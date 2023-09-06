using PIMES_DMS.Data;
using System.Net;
using System.Net.Mail;
using PIMES_DMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace PIMES_DMS.Controllers
{
    public class NotifController : Controller
    {
        private readonly AppDbContext _Db;

        public NotifController(AppDbContext db)
        {
            _Db = db;
        }

        public IActionResult ShowNotif()
        {
            string? EN = TempData["EN"] as string;
            TempData.Keep();

            if (string.IsNullOrEmpty(EN))
            {
                return RedirectToAction("Logout", "Login");
            }

            return View(_Db.NotifDb.OrderByDescending(j => j.DateCreated));
        }

        public IActionResult ClearNotificationBtn()
        {
            foreach (var notif in _Db.NotifDb)
            {
                _Db.NotifDb.Remove(notif);
            }

            _Db.SaveChanges();

            return RedirectToAction("ShowNotif");
        }

    }
}


