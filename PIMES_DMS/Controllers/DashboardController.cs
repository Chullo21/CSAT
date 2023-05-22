using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PIMES_DMS.Data;
using System.Globalization;

namespace PIMES_DMS.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _Db;

        public DashboardController(AppDbContext db)
        {
            _Db = db;
        }

        public IActionResult DashView()
        {
            GetAllData(DateTime.Now.Year);
            
            var currentDate = DateTime.Now;
            var currentMonth = currentDate.Month;
            TempData["Months"] = currentMonth - 1;

            var months = new string[currentMonth];
            var dateTimeFormatInfo = CultureInfo.CurrentCulture.DateTimeFormat;

            for (var i = 1; i <= currentMonth; i++)
            {
                var monthName = dateTimeFormatInfo.GetMonthName(i);
                months[i - 1] = monthName;

            }

            ViewBag.Month = JsonConvert.SerializeObject(months);
           

            return View();
        }

        public void GetAllData(int year)
        {
            int currentMonth = DateTime.Now.Month;
            int[] valids = new int[currentMonth + 1];
            int[] invalids = new int[currentMonth + 1];
            
            for (int i = 1; i <= currentMonth; i++)
            {
                DateTime YYear = new DateTime(year, i, 1);
                valids[i] = _Db.IssueDb.Count(j => j.CoD == "8D" && !j.isDeleted && j.DateCreated.Year == YYear.Year && j.DateCreated.Month == YYear.Month);
                invalids[i] = _Db.IssueDb.Count(m => !m.isDeleted && m.DateCreated.Year == YYear.Year && m.DateCreated.Month == YYear.Month && (m.CoD == "CAPA" || m.CoD == "INV"));
            }

            int[] newValids = new int[currentMonth];
            int[] newInvalids = new int[currentMonth];
            Array.Copy(valids, 1, newValids, 0, currentMonth);
            Array.Copy(invalids, 1, newInvalids, 0, currentMonth);

            var openData = _Db.VerDb.Count(o => o.Status == "o" && !o.IsDeleted && o.DateVer.Year == year);
            var closedData = _Db.VerDb.Count(o => o.Status == "c" && !o.IsDeleted && o.DateVer.Year == year);
            var goingData = _Db.VerDb.Count(o => o.Status == "g" && !o.IsDeleted && o.DateVer.Year == year);

            ViewBag.valid = JsonConvert.SerializeObject(newValids);
            ViewBag.invalid = JsonConvert.SerializeObject(newInvalids);

            ViewBag.open = JsonConvert.SerializeObject(openData);
            ViewBag.closed = JsonConvert.SerializeObject(closedData);
            ViewBag.going = JsonConvert.SerializeObject(goingData);
        }

        public IActionResult RecentYear(int year)
        {
            GetAllData(year);
            
            if (DateTime.Now.Year != year)
            {
                TempData["Months"] = 12;
            }
            else
            {
                var currentDate = DateTime.Now;
                TempData["Months"] = currentDate.Month - 1;
            }


            var months = new string[12];
            var dateTimeFormatInfo = CultureInfo.CurrentCulture.DateTimeFormat;

            for (var i = 1; i <= 12; i++)
            {
                var monthName = dateTimeFormatInfo.GetMonthName(i);
                months[i - 1] = monthName;
            }

            ViewBag.Month = JsonConvert.SerializeObject(months);

            return View("DashView");
        }
    }
}
