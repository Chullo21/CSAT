using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult ValidateView(int ID)
        {
            IssueModel val = _Db.IssueDb.Find(ID);

            return View(val);
        }

        public IActionResult SubmitValidation(int id, string controlno, string validation, string valsumrep)
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
                val.ControlNumber = controlno;
                val.CoD = validation;
                val.ValidatedStatus = true;
                val.ValidationRepSum = valsumrep;
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

        public IActionResult ShowIssuesWithReport(int ID)
        {
            string Role = TempData["Role"] as string;
            string EN = TempData["EN"] as string;
            TempData.Keep();


            if (Role == "CLIENT")
            {
                IEnumerable<IssueModel> det = _Db.IssueDb.Where(j => j.IssueCreator == EN && j.ValidatedStatus);

                return View(det);
            }
            else
            {
                IEnumerable<IssueModel> det1 = _Db.IssueDb.Where(j => j.ValidatedStatus);
                return View(det1);
            }

        }

        public IActionResult ValidatedIssueDetail(int id)
        {

            IssueModel det = _Db.IssueDb.Find(id);

            return View(det);
        }
    }
}
