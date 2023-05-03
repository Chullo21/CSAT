using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using PIMES_DMS.Data;
using PIMES_DMS.Models;

namespace PIMES_DMS.Controllers
{
    public class IssueController : Controller
    {

        private readonly AppDbContext _context;

        public IssueController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult SubmitIssueView()
        {
            return View();
        }

        public IActionResult SubmitIssue(string issueno,string datefound, string product, int serialno, string affectedpn, string description, string problemdescription)
        {

            string creator = TempData["EN"] as string;
            TempData.Keep();

            IssueModel issue = new IssueModel();
            {
                issue.IssueNo = issueno;
                issue.IssueCreator = creator;
                issue.DateFound = datefound;
                issue.Product = product;
                issue.SerialNo = serialno;
                issue.AffectedPN = affectedpn;
                issue.Desc = description;
                issue.ProbDesc = problemdescription;
            }

            if (ModelState.IsValid)
            {
                _context.IssueDb.Add(issue);
                _context.SaveChanges();
            }

            return View("IssueDetails", issue);
        }

        public IActionResult IssuesList()
        {          
           if (TempData.ContainsKey("Role"))
            {
                string Role = TempData["Role"] as string;
                TempData.Keep();

                string user = TempData["EN"] as string;
                TempData.Keep();

                if (Role == "ADMIN")
                {
                    var issues = _context.IssueDb.Where(j => j.ValidatedStatus == false);
                    return View(issues);
                }
                else if (Role == "CLIENT")
                {
                    var issues = _context.IssueDb.Where(j => j.IssueCreator == user && j.ValidatedStatus == false);
                    return View(issues);
                }
            }

            return NotFound();
        }
        
        public IActionResult IssueDetails(int ID)
        {
            IssueModel det = _context.IssueDb.Find(ID);
            
            return View(det);
        }

        public IActionResult AcknowledgeIssue(int ID)
        {
            var issue = _context.IssueDb.Find(ID);

            IssueModel ackissue = new IssueModel();
            {
                ackissue = issue;
                ackissue.Acknowledged = true;
            }

            if (ModelState.IsValid)
            {
                _context.IssueDb.Update(ackissue);
                _context.SaveChanges();

                return View("IssueDetails", ackissue);
            }

            return RedirectToAction("IssuesList");
        }
    }
}
