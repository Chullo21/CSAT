﻿using Microsoft.AspNetCore.Mvc;
using PIMES_DMS.Data;
using PIMES_DMS.Models;
using System.Drawing;

namespace PIMES_DMS.Controllers
{
    public class ValidationController : Controller
    {
        private readonly AppDbContext _Db;

        public ValidationController(AppDbContext controller)
        {
            _Db = controller;
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
                _Db.SaveChangesAsync();
            }
        }

        public IActionResult SaveEmailSnip(int ID, IFormFile img)
        {
            var issue = _Db.IssueDb.FirstOrDefault(j => j.IssueID == ID);
            IssueModel model = new IssueModel();
            {
                model = issue;

            }
            using(MemoryStream ms =  new MemoryStream())
            {
                img.CopyTo(ms);
                model.EmailSnip = ms.ToArray();
            }

            _Db.IssueDb.Update(model);
            _Db.SaveChanges();

            return RedirectToAction("ValidatedIssueDetail", ID);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult ValidateView(int ID)
        {
            IssueModel? val = _Db.IssueDb.Find(ID);

            return View(val);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult ShowIssuesWithReport()
        {
            string? Role = TempData["Role"] as string;
            TempData.Keep();

            if (Role == "CLIENT")
            {
                string? EN = TempData["EN"] as string;
                TempData.Keep();

                IEnumerable<IssueModel> det = _Db.IssueDb.Where(j => j.IssueCreator == EN && j.ValidatedStatus && j.Acknowledged);

                return View(det);
            }
            else
            {
                IEnumerable<IssueModel> det1 = _Db.IssueDb.Where(j => j.ValidatedStatus && j.Acknowledged);
                return View(det1);
            }

        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult ValidatedIssueDetail(int id)
        {
            IssueModel? det = _Db.IssueDb.FirstOrDefault(j => j.IssueID == id);

            if (det.ValRes == "Invalid")
            {
                string stringforEmail = Convert.ToBase64String(det.EmailSnip);
                ViewBag.EmailSnip = "data:image/jpeg;base64," + stringforEmail;
            }

            return View(det);

        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult ShowPdf(int id, string type)
        {
            var details = _Db.IssueDb.Find(id);

            byte[]? docinByte = details!.Report;

            return File(docinByte!, "application/pdf");      
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult ForGERVal()
        {
            IEnumerable<IssueModel> obj = _Db.IssueDb.Where(j => (j.Acknowledged && j.ValidatedStatus && !j.HasCR && j.ValRes == "Valid")
            || (j.Acknowledged && j.ValidatedStatus && !j.HasCR && j.ValRes == "Invalid" && j.NeedRMA == "Yes"));

            return View(obj);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult ShowClientandQARep(int ID)
        {
            return View(_Db.IssueDb.FirstOrDefault(j => j.IssueID == ID));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult SubmitValidation(int id, string validation, string? valsumrep, IFormFile? valrep, string? nrma, DateTime dateval, IFormFile? emailimg)
        {
            string? EN = TempData["EN"] as string;
            TempData.Keep();

            var fromDb = _Db.IssueDb.FirstOrDefault(j => j.IssueID == id);

            if (fromDb == null)
            {
                return NotFound();
            }

            if (valsumrep == "Report here") valsumrep = "No Summary";

            var val = new IssueModel();
            {
                val = fromDb;
                val.ValNo = GetUniqueNumberForVR(fromDb);
                val.ValRes = validation;
                val.ValidatedStatus = true;
                val.ValidationRepSum = valsumrep!;
                val.DateVdal = dateval;
                val.NeedRMA = nrma;
                val.Validator = EN!;
            }

            if (validation == "Valid")
            {
                val.NeedRMA = "Yes";
                val.ControlNumber = GetUniqueNumberFor8D(fromDb);
            }

            if (valrep != null)
            {
                using MemoryStream ms = new();
                valrep.CopyTo(ms);
                val.Report = ms.ToArray();
            }

            if (emailimg != null)
            {
                using MemoryStream ms = new();
                emailimg.CopyTo(ms);
                val.EmailSnip = ms.ToArray();
            }

            if (ModelState.IsValid)
            {
                _Db.IssueDb.Update(val);
                _Db.SaveChanges();

                UpdateNotif(", have sumitted a validation report." + val.ValNo, "All");

                ValidatedIssueDetail(id);
            }

            return View("ValidatedIssueDetail", val);
        }

        private string GetUniqueNumberForVR(IssueModel issue)
        {
            List<IssueModel> issues = _Db.IssueDb.Where(j => !string.IsNullOrEmpty(j.ValNo) && j.DateFound.Year == issue.DateFound.Year).ToList();

            issues = issues.OrderByDescending(j => j.DateFound).ToList();

            int series = 1;

            if (issues.Count > 0)
            {
                series = int.Parse(issues.First().ValNo!.Substring(6, 3)) + 1;
            }

            return "VR-" + issue.DateFound.Year.ToString().Substring(2, 2) + "-" + series.ToString("000");
        }

        private string GetUniqueNumberFor8D(IssueModel issue)
        {
            List<IssueModel> issues = _Db.IssueDb.Where(j => !string.IsNullOrEmpty(j.ControlNumber) && j.DateFound.Year == issue.DateFound.Year).ToList();

            issues = issues.OrderByDescending(j => j.DateFound).ToList();

            int series = 1;

            if (issues.Count > 0)
            {
                series = int.Parse(issues.First().ValNo!.Substring(6, 3)) + 1;
            }

            return "8D-" + issue.DateFound.Year.ToString().Substring(2, 2) + "-" + series.ToString("000");
        }

    }
}
