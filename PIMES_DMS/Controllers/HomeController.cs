using Microsoft.AspNetCore.Mvc;
using PIMES_DMS.Data;
using PIMES_DMS.Models;
using System.Diagnostics;
using System.Net.Mail;

namespace PIMES_DMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _Db;

        public HomeController(AppDbContext db)
        {
            _Db = db;
        }

        public void UpdateNotif(string message, string t)
        {
            string? EN = TempData["EN"] as string;
            TempData.Keep();

            NotifModel nm = new NotifModel();
            {
                nm.Message = EN + message;
                nm.DateCreated = DateTime.Now;
                nm.Type = t;
            }

            if (ModelState.IsValid)
            {
                _Db.NotifDb.Add(nm);
            }
        }

        public IActionResult Home()
        {
            return View();
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult AdminHome()
        {

            ViewData["InComingIssue"] = _Db.IssueDb.Where(j => !j.Acknowledged && !j.ValidatedStatus);
            ViewData["OnProgressIssue"] = _Db.IssueDb.Where(j => j.Acknowledged && !j.ValidatedStatus);

            ViewBag.Claims = GetNumberOfClaims();
            ViewBag.OnProcess = GetNumberOfOnProcess();
            ViewBag.ERA = GetNumberOfERA();
            ViewBag.RC = GetNumberOfRC();

            return View();
        }

        private int GetNumberOfClaims()
        {
            return _Db.IssueDb.Count(j => !j.Acknowledged && !j.ValidatedStatus);
        }

        private int GetNumberOfOnProcess()
        {
            return _Db.IssueDb.Count(j => j.Acknowledged && !j.ValidatedStatus);
        }

        private int GetNumberOfERA()
        {
            return _Db.IssueDb.Count(j => j.Acknowledged && j.ValidatedStatus && !j.HasCR);
        }

        private int GetNumberOfRC()
        {
            List<IssueModel> issues = _Db.IssueDb.Where(j => j.HasCR && j.ValRes == "Valid").ToList();
            List<IssueModel> issuestoshow = new List<IssueModel>();

            foreach (var issue in issues)
            {
                List<ActionModel> actions = _Db.ActionDb.Where(j => j.ControlNo == issue.ControlNumber).ToList();

                if (!actions.All(j => j.ActionStatus == "Closed") || actions.Count == 0)
                {
                    issuestoshow.Add(issue);
                }
            }
            return issuestoshow.Count;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult CheckFor8D(string controlno, IFormFile attachment)
        {
            TempData["Existing8D"] = null;

            _8DModel? existing8D = _Db._8DDb.FirstOrDefault(j => j.ControlNo == controlno);           

            if (existing8D != null)
            {
                TempData["Existing8D"] = "You have replaced an existing 8D attachment with control number of " + controlno;

                Update8D(controlno, attachment);

                return RedirectToAction("AdminHome");
            }
            else
            {
                Upload8D(controlno, attachment);

                return RedirectToAction("AdminHome");
            }           
        }

        private void Update8D(string controlno, IFormFile attachment)
        {
            _8DModel model = new _8DModel();
            {
                model.ControlNo = controlno;

                using (MemoryStream ms = new MemoryStream())
                {
                    attachment.CopyTo(ms);
                    model.Report = ms.ToArray();
                }
            }

            if (ModelState.IsValid)
            {
                UpdateNotif(", replaced an existing 8D attachment with control number of " + controlno, "Admins");
                _Db._8DDb.Update(model);
                _Db.SaveChanges();
            }
        }

        public void Upload8D(string controlno, IFormFile attachment)
        {
            //TempData["Existing8D"] = null;

            //_8DModel? existing8D = _Db._8DDb.FirstOrDefault(j => j.ControlNo == controlno);

            //TempData["Existing8D"] = "EXISTING CONTROL NUMBER";

            //if (existing8D != null)
            //{
            //    return RedirectToAction("AdminHome");
            //}

            _8DModel model = new _8DModel();
            {
                model.ControlNo = controlno;

                using (MemoryStream ms = new MemoryStream())
                {
                    attachment.CopyTo(ms);
                    model.Report = ms.ToArray();
                }
            }

            if (ModelState.IsValid)
            {
                UpdateNotif(", uploaded an 8D attachment with control number of " + controlno, "Admins");
                _Db._8DDb.Add(model);
                _Db.SaveChanges();
            }

            //return RedirectToAction("AdminHome");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}