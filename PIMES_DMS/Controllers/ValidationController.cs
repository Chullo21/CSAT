using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PIMES_DMS.Data;
using PIMES_DMS.Models;

namespace PIMES_DMS.Controllers
{
    public class ValidationController : Controller
    {
        private readonly AppDbContext _Db;

        public ValidationController(AppDbContext controller)
        {
            _Db = controller;
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult ValidateView(int ID)
        {
            IssueModel? val = _Db.IssueDb.Find(ID);

            return View(val);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult SubmitValidation(int id, string validation, string valsumrep, IFormFile? valrep)
        {
            var fromDb = _Db.IssueDb.FirstOrDefault(j => j.IssueID == id);

            if (fromDb == null)
            {
                return NotFound();
            }

            if (valsumrep == "Report here") valsumrep = "No Summary";

            var val = new IssueModel();
            {
                val = fromDb;
                val.ControlNumber = validation + "-" + DateTime.Now.ToString("yy") + "-" + fromDb.IssueID.ToString("000");
                val.CoD = validation;
                val.ValidatedStatus = true;
                val.ValidationRepSum = valsumrep;
                val.DateVdal = DateTime.Now;
            }

            if (valrep != null)
            {
                using MemoryStream ms = new();
                valrep.CopyTo(ms);
                val.Report = ms.ToArray();
            }

            if (ModelState.IsValid)
            {
                _Db.Entry(fromDb).State = EntityState.Detached;

                _Db.Entry(val).State = EntityState.Modified;

                _Db.SaveChanges();

                ValidatedIssueDetail(id);
            }

            return View("ValidatedIssueDetail", val);
        }

        public IActionResult InvalidRep()
        {
            return View();
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

            IssueModel? det = _Db.IssueDb.Find(id);

            return View(det);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult SearchVal(string ss)
        {
            string? Role = TempData["Role"] as string;
            TempData.Keep();

            if (ss.IsNullOrEmpty())
            {
                return View("ShowIssuesWithReport", _Db.IssueDb.Where(j => j.Acknowledged && j.ValidatedStatus));
            }

            if (Role == "CLIENT")
            {

                string? EN = TempData["EN"] as string;
                TempData.Keep();

                IEnumerable<IssueModel> obj = from m in _Db.IssueDb
                                              where m.IssueCreator == EN && m.ValidatedStatus && m.Acknowledged &&
                                              (m.IssueNo.Contains(ss) || m.AffectedPN.Contains(ss) || m.Desc.Contains(ss) ||
                                              m.SerialNo.ToString().Contains(ss) || m.ControlNumber.Contains(ss))
                                              select m;

                return View("ShowIssuesWithReport", obj);
            }
            else
            {
                IEnumerable<IssueModel> obj = _Db.IssueDb.Where(m => m.ValidatedStatus && m.Acknowledged && (m.IssueCreator.Contains(ss)
                                              || m.IssueNo.Contains(ss) || m.AffectedPN.Contains(ss) || m.Desc.Contains(ss) ||
                                              m.SerialNo.ToString().Contains(ss) || m.ControlNumber.Contains(ss)));

                return View("ShowIssuesWithReport", obj);
            }
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult ShowPdf(int id, string type)
        {
            var details = _Db.IssueDb.Find(id);

            if (type == "Client")
            {
                byte[]? docinByte = details!.ClientRep;

                return File(docinByte!, "application/pdf");
            }
            else
            {
                byte[]? docinByte = details!.Report;

                return File(docinByte!, "application/pdf");
            }           
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult ForGERVal()
        {
            IEnumerable<IssueModel> obj = _Db.IssueDb.Where(j => j.Acknowledged && j.ValidatedStatus && !j.HasCR);

            return View(obj);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult ShowClientandQARep(int ID)
        {
            return View(_Db.IssueDb.FirstOrDefault(j => j.IssueID == ID));
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult Search(string ss)
        {
            string? Role = TempData["Role"] as string;
            TempData.Keep();

            if (ss.IsNullOrEmpty())
            {
                return View("ForGERVal", _Db.IssueDb.Where(j => j.Acknowledged && j.ValidatedStatus));
            }

            if (Role == "CLIENT")
            {

                string? EN = TempData["EN"] as string;
                TempData.Keep();

                IEnumerable<IssueModel> obj = from m in _Db.IssueDb
                                              where m.IssueCreator == EN && m.ValidatedStatus && m.Acknowledged &&
                                              (m.IssueNo.Contains(ss) || m.AffectedPN.Contains(ss) || m.Desc.Contains(ss) ||
                                              m.SerialNo.ToString().Contains(ss) || m.ControlNumber.Contains(ss))
                                              select m;

                return View("ForGERVal", obj);
            }
            else
            {
                IEnumerable<IssueModel> obj = _Db.IssueDb.Where(m => m.ValidatedStatus && m.Acknowledged && (m.IssueCreator.Contains(ss)
                                              || m.IssueNo.Contains(ss) || m.AffectedPN.Contains(ss) || m.Desc.Contains(ss) ||
                                              m.SerialNo.ToString().Contains(ss) || m.ControlNumber.Contains(ss)));

                return View("ForGERVal", obj);
            }
        }

    }
}
