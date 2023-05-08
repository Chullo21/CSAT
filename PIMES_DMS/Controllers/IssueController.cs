﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

        public IActionResult SubmitIssue(string issueno, string datefound, string product, int serialno, string affectedpn, string description, string problemdescription, IFormFile? cp)
        {
            string? creator = TempData["EN"] as string;
            TempData.Keep();

            IssueModel issue = new();
            {
                issue.IssueNo = issueno;
                issue.IssueCreator = creator!;
                issue.DateFound = datefound;
                issue.Product = product;
                issue.SerialNo = serialno;
                issue.AffectedPN = affectedpn;
                issue.Desc = description;
                issue.ProbDesc = problemdescription;
            }
            
            if (cp != null)
            {
                using MemoryStream memoryStream = new();
                cp.CopyTo(memoryStream);
                issue.ClientRep = memoryStream.ToArray();
            }

            if (ModelState.IsValid)
            {
                _context.IssueDb.Add(issue);
                _context.SaveChanges();

                return View("IssueDetails", issue);
            }

            return RedirectToAction("IssuesList");
        }

        public IActionResult ShowPdf(int ID)
        {
            var details = _context.IssueDb.Find(ID);

            byte[]? docinByte = details!.ClientRep;

            return File(docinByte!, "application/pdf");
        }

        public IActionResult IssuesList()
        {
            if (TempData.ContainsKey("Role"))
            {
                string? Role = TempData["Role"] as string;
                TempData.Keep();               

                if (Role == "CLIENT")
                {
                    string? user = TempData["EN"] as string;
                    TempData.Keep();

                    var issues = _context.IssueDb.Where(j => j.IssueCreator == user && !j.ValidatedStatus);
                    return View(issues);
                }
                else
                {
                    var issues = _context.IssueDb.Where(j => !j.ValidatedStatus);
                    return View(issues);
                }
            }

            return NotFound();
        }

        public IActionResult AcknowledgedIssues()
        {
            string? Role = TempData["Role"] as string;
            TempData.Keep();

            if (Role == "CLIENT")
            {
                string? EN = TempData["EN"] as string;
                TempData.Keep();

                IEnumerable<IssueModel> obj = _context.IssueDb.Where(j => j.IssueCreator == EN && j.Acknowledged && !j.ValidatedStatus);

                return View("AckIssues", obj);
            }
            else
            {
                IEnumerable<IssueModel> obj = _context.IssueDb.Where(j => j.Acknowledged && !j.ValidatedStatus);

                return View("AckIssues", obj);
            }
        }

        public IActionResult IssueDetails(int ID)
        {
            IssueModel? det = _context.IssueDb.Find(ID);

            return View(det);
        }

        public IActionResult AcknowledgeIssue(int ID)
        {
            var issue = _context.IssueDb.Find(ID);

            IssueModel ackissue = new();
            {
                ackissue = issue!;
                ackissue.Acknowledged = true;
                ackissue.DateAck = DateTime.Now;
            }

            if (ModelState.IsValid)
            {
                _context.IssueDb.Update(ackissue);
                _context.SaveChanges();

                return View("IssueDetails", ackissue);
            }

            return RedirectToAction("IssuesList");
        }

        public IActionResult Search(string ss)
        {
            string? Role = TempData["Role"] as string;
            string? EN = TempData["EN"] as string;
            TempData.Keep();

            if (ss.IsNullOrEmpty())
            {
                return RedirectToAction("IssuesList");
            }

           if (Role == "CLIENT")
            {
                IEnumerable<IssueModel> obj = from m in _context.IssueDb where m.IssueCreator == EN && !m.ValidatedStatus &&
                                              (m.IssueNo.Contains(ss) || m.AffectedPN.Contains(ss) || m.Desc.Contains(ss) || 
                                              m.SerialNo.ToString().Contains(ss)) select m;

                return View("IssuesList", obj);
            }
            else
            {
                IEnumerable<IssueModel> obj = from m in _context.IssueDb where !m.ValidatedStatus && m.IssueCreator.Contains(ss) 
                                              || (m.IssueNo.Contains(ss) || m.AffectedPN.Contains(ss) || m.Desc.Contains(ss) || 
                                              m.SerialNo.ToString().Contains(ss))
                                              select m;

                return View("IssuesList", obj);
            }
        }

    }    

    
}
