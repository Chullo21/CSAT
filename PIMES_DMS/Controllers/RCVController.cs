using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PIMES_DMS.Data;
using PIMES_DMS.Models;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace PIMES_DMS.Controllers
{
    public class RCVController : Controller
    {
        private readonly AppDbContext _Db;

        public RCVController(AppDbContext db)
        {
            _Db = db;
        }

        public IActionResult RCVList()
        {
            string? Role = TempData["Role"] as string;
            TempData.Keep();

            if (Role == "CLIENT")
            {
                string? EN = TempData["EN"] as string;
                TempData.Keep();

                return View(_Db.IssueDb.Where(j => j.Acknowledged && j.ValidatedStatus && j.HasCR && j.IssueCreator == EN && !j.isDeleted));
            }
            else
            {
                return View(_Db.IssueDb.Where(j => j.Acknowledged && j.ValidatedStatus && j.HasCR && !j.isDeleted));
            }

        }

        public IActionResult RCVView(int ID)
        {
            return View(_Db.IssueDb.FirstOrDefault(j => j.IssueID == ID));
        }

        public IActionResult SubmitRCV(string controlno, List<string> action, List<string> pic, List<string> tsc, List<string> ec, List<DateTime> td)
        {
            action.RemoveAll(line => line == null);

            //var issue = _Db.IssueDb.FirstOrDefault(j => j.ControlNumber == controlno);

            //var obj = new SumRepModel();
            //{
            //    obj.IssueNo = issue!.IssueNo;
            //    obj.Product = issue.Product;
            //    obj.AffectedQty = issue.Qty;
            //    obj.ProblemDesc = issue.ProbDesc;
            //}

            //if (ModelState.IsValid)
            //{
            //    _Db.SRDb.Add(obj);
            //    _Db.SaveChanges();
            //}

            for (int i = 0; i < action.Count; i++)
            {
                var act = new ActionModel();
                {
                    act.ControlNo = controlno;
                    act.Action = action[i];
                    act.PIC = pic[i];
                    act.TSC = tsc[i];
                    act.EC = ec[i];
                    act.TargetDate = td[i];
                }

                _Db.ActionDb.Add(act);
                _Db.SaveChanges();
            }


            return RedirectToAction("RCVList");
        }

        public IActionResult Search(string ss)
        {
            if (ss == null)
            {
                return RedirectToAction("RCVList");
            }

            var list = _Db.IssueDb.Where(j => j.ControlNumber.Contains(ss) && !j.isDeleted);

            return View("RCVList", list);
        }
    }
}
