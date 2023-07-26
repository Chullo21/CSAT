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

        public IActionResult Upload8D(string controlno, IFormFile attachment)
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
                _Db._8DDb.Add(model);
                _Db.SaveChanges();
            }

            return RedirectToAction("AdminHome");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}