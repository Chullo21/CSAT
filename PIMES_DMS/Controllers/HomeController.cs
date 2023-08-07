﻿using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Search(string type, string ss)
        {
            switch (type)
            {
                case "IssueDetail":
                    {
                        IssueModel? issue = _Db.IssueDb.FirstOrDefault(j => j.ControlNumber == ss || j.IssueNo == ss);
                        
                        if (issue != null)
                        {
                            return View("~/Views/Issue/IssueDetails.cshtml", issue);
                        }
                        else
                        {
                            TempData["Existing8D"] = "Search failed, no '" + ss + "' was found.";
                            return RedirectToAction("AdminHome");
                        }
                        
                    }
                case "VR":
                    {
                        IssueModel? issue = _Db.IssueDb.FirstOrDefault(j => (j.ControlNumber == ss || j.IssueNo == ss) && j.ValidatedStatus);

                        if (issue != null)
                        {
                            return View("~/Views/Validation/ValidatedIssueDetail.cshtml", issue);
                        }
                        else
                        {
                            TempData["Existing8D"] = "Search failed, no '" + ss + "' was found.";
                            return RedirectToAction("AdminHome");
                        }
                    }
                case "CAPA":
                    {
                        IssueModel? issue = _Db.IssueDb.FirstOrDefault(j => j.ControlNumber == ss && j.HasTES);

                        if (issue != null)
                        {
                          
                            return RedirectToAction("TESActions", "RCV", new { ID = issue.ControlNumber });
                        }
                        else
                        {
                            TempData["Existing8D"] = "Search failed, no '" + ss + "' was found. Please input control number for CAPA.";
                            return RedirectToAction("AdminHome");
                        }
                    }
                default:
                    {
                        return View(type, ss);
                    }
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
            return _Db.IssueDb.Count(j => !j.Acknowledged);
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

            var CheckForIssueWithCN = _Db.IssueDb.FirstOrDefault(j => j.ControlNumber == controlno);

            if (CheckForIssueWithCN == null)
            {
                TempData["Existing8D"] = "We have no QI with control number of '" + controlno + "'. Please put the control number correctly.";
                return RedirectToAction("AdminHome");
            }

            _8DModel? existing8D = _Db._8DDb.FirstOrDefault(j => j.ControlNo == controlno);

            if (existing8D != null)
            {
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
            _8DModel mod = _Db._8DDb.FirstOrDefault(j => j.ControlNo == controlno)!;

            _8DModel model = new _8DModel();
            {
                model = mod!;

                using (MemoryStream ms = new MemoryStream())
                {
                    attachment.CopyTo(ms);
                    model.Report = ms.ToArray();
                }
            }

            if (ModelState.IsValid)
            {
                TempData["Existing8D"] = "You have replaced an existing 8D attachment with control number of " + controlno;
                UpdateNotif(", replaced an existing 8D attachment with control number of " + controlno, "Admins");
                _Db._8DDb.Update(model);
                _Db.SaveChanges();
            }
        }

        public void Upload8D(string controlno, IFormFile attachment)
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
                TempData["Existing8D"] = "You have uploaded an 8D attachment with control number of " + controlno;
                UpdateNotif(", uploaded an 8D attachment with control number of " + controlno, "Admins");
                CheckIfIssueHave8D(controlno);
                _Db._8DDb.Add(model);
                _Db.SaveChanges();
            }
        }

        private void CheckIfIssueHave8D(string controlno)
        {
            IssueModel issue = _Db.IssueDb.FirstOrDefault(j => j.ControlNumber == controlno);           

            if (issue != null)
            {
                IssueModel updateIssue = new IssueModel();
                {
                    updateIssue = issue;
                    updateIssue.Has8D = true;
                }

                _Db.IssueDb.Update(updateIssue);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}