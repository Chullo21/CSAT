using Microsoft.AspNetCore.Mvc;
using PIMES_DMS.Data;
using PIMES_DMS.Models;

namespace PIMES_DMS.Controllers
{
    public class RCVController : Controller
    {
        private readonly AppDbContext _Db;

        public RCVController(AppDbContext db)
        {
            _Db = db;
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult RCVList()
        {
            string? Role = TempData["Role"] as string;
            TempData.Keep();

            if (Role == "CLIENT")
            {
                string? EN = TempData["EN"] as string;
                TempData.Keep();

                return View(_Db.IssueDb.Where(j => j.IssueCreator == EN && j.Acknowledged && j.ValidatedStatus && j.HasCR && !j.isDeleted));
            }

            ViewData["Stat"] = _Db.VerDb.Where(j => !j.IsDeleted);


            return View(_Db.IssueDb.Where(j => j.Acknowledged && j.ValidatedStatus && j.HasCR && !j.isDeleted));
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult RCVView(int ID)
        {
            return View(_Db.IssueDb.FirstOrDefault(j => j.IssueID == ID));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult SubmitRCV(string controlno, List<string> action, List<string> pic, List<string> tsc, List<string> ec, List<DateTime> td)
        {
            action.RemoveAll(line => line == null);

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

                if (ModelState.IsValid)
                {
                    _Db.ActionDb.Add(act);
                    _Db.SaveChanges();                   
                }

            }

            var iss = _Db.IssueDb.FirstOrDefault(i => i.ControlNumber == controlno);
            iss!.HasAction = true;

            _Db.IssueDb.Update(iss);
            _Db.SaveChanges();


            return RedirectToAction("RCVDet", "RCV", new { ID = controlno });
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult Search(string ss)
        {
            ViewData["Stat"] = _Db.VerDb.Where(j => !j.IsDeleted);

            if (ss == null)
            {               
                return RedirectToAction("RCVList", _Db.IssueDb.Where(j => j.Acknowledged && j.ValidatedStatus && j.HasCR && !j.isDeleted));
            }

            string? Role = TempData["Role"] as string;
            TempData.Keep();

            if (Role == "CLIENT")
            {
                string? EN = TempData["EN"] as string;
                TempData.Keep();

                IEnumerable<IssueModel> obj = from m in _Db.IssueDb
                                              where m.IssueCreator == EN && m.ValidatedStatus && m.Acknowledged && m.HasCR &&
                                              (m.IssueNo.Contains(ss) || m.ControlNumber.Contains(ss))
                                              select m;

                return View("RCVList", obj);
            }
            else
            {
                IEnumerable<IssueModel> obj = _Db.IssueDb.Where(m => m.ValidatedStatus && m.Acknowledged && m.HasCR && (m.IssueCreator.Contains(ss)
                                              || m.IssueNo.Contains(ss) || m.ControlNumber.Contains(ss)));

                return View("RCVList", obj);
            }

        }

        [AutoValidateAntiforgeryToken]
        public IActionResult RCVDet(string ID)
        {

            var issue = _Db.IssueDb.FirstOrDefault(j => j.ControlNumber == ID);

            IEnumerable<ActionModel> actions = _Db.ActionDb.Where(i => i.ControlNo == ID && !i.IsDeleted);
            
            foreach (ActionModel action in actions)
            {
                var ver = _Db.VerDb.Count(j => j.ActionID == action.ActionID);
                if (ver == 0)
                {
                    var act = _Db.ActionDb.FirstOrDefault(j => j.ActionID == action.ActionID);

                    act!.HasVer = false;

                    _Db.ActionDb.Update(act);
                    _Db.SaveChanges();
                }
            }

            if (issue != null && actions != null)
            {
                ViewData["Issue"] = issue;
                ViewData["Action"] = actions;
            }

            return View("RCVDet");
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult ClientViewRCV()
        {
            string? Role = TempData["Role"] as string;
            TempData.Keep();

            if (Role == "CLIENT")
            {
                string? EN = TempData["EN"] as string;
                TempData.Keep();

                return View(_Db.IssueDb.Where(j => j.Acknowledged && j.ValidatedStatus && j.HasCR && j.IssueCreator == EN && !j.isDeleted && j.HasAction));
            }
            else
            {
                return View(_Db.IssueDb.Where(j => j.Acknowledged && j.ValidatedStatus && j.HasCR && !j.isDeleted && j.HasAction));
            }
        }

        [AutoValidateAntiforgeryToken]
        public IActionResult RCVDelete(int ID)
        {
            var del = _Db.ActionDb.FirstOrDefault(j => j.ActionID == ID);
            var vals = _Db.VerDb.Where(j => j.ActionID == ID && j.ControlNo == del!.ControlNo);


            string? contno = del!.ControlNo;

            if (del != null)
            {

                foreach(var kvp in vals)
                {
                    kvp.IsDeleted = true;

                    _Db.VerDb.Update(kvp);
                }

                del.IsDeleted = true;

                
                _Db.ActionDb.Update(del);
                _Db.SaveChanges();
            }

            var check = _Db.ActionDb.Where(m => m.ControlNo == contno && !m.IsDeleted);

            if (!check.Any())
            {
                var issue = _Db.IssueDb.FirstOrDefault(i => i.ControlNumber == contno);

                if (issue!.HasAction)
                {
                    issue.HasAction = false;

                    _Db.IssueDb.Update(issue);
                    _Db.SaveChanges();
                }
            }

            return RedirectToAction("RCVDet", new {ID = contno});
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult VerRCVView(int ID)
        {
            var action = _Db.ActionDb.FirstOrDefault(j => j.ActionID == ID && !j.IsDeleted);

            if (action == null)
            {
                return NotFound();
            }

            ViewData["Action"] = action;
            ViewData["Issue"] = _Db.IssueDb.FirstOrDefault(m => m.ControlNumber == action!.ControlNo);

            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult VerRCV(int id, string? result, DateTime date, IFormFile? evidence, string status)
        {
            var up = _Db.ActionDb.FirstOrDefault(j => j.ActionID == id);
            up!.HasVer = true;

            var ver = new Vermodel();
            {
                ver.ActionID = up.ActionID;
                ver.ControlNo = up.ControlNo!;
                ver.Result = result;
                ver.DateVer = date;
                ver.Status = status;
                ver.IsVer = true;
            }
            

            if (evidence != null)
            {
                using MemoryStream ms = new();
                evidence.CopyTo(ms);
                ver.Files = ms.ToArray();
            }

            _Db.ActionDb.Update(up);
            _Db.VerDb.Add(ver);
            _Db.SaveChanges();

            return RedirectToAction("RCVDet", "RCV", new { ID = up.ControlNo });
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult RCVViewDet(int ID)
        {
            ViewData["Ver"] = null;

            var action = _Db.ActionDb.FirstOrDefault(j => j.ActionID == ID && !j.IsDeleted);

            if (action == null)
            {
                return NotFound();
            }

            ViewData["Action"] = action;
            ViewData["Issue"] = _Db.IssueDb.FirstOrDefault(m => m.ControlNumber == action!.ControlNo);
            ViewData["Ver"] = _Db.VerDb.Where(i => i.ActionID == ID);

            return View();

        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult ShowFile(int ID)
        {
            var file = _Db.VerDb.FirstOrDefault(j => j.VerID == ID);

            if (file!.Files == null)
            {
                return NoContent();
            }

            return File(file!.Files, "application/pdf");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult UpdateVER(int ID, DateTime datever, string? status, string? Uresult, IFormFile? file)
        {
            var model = _Db.VerDb.FirstOrDefault(m => m.VerID == ID);
            var obj = _Db.ActionDb.FirstOrDefault(j => j.ControlNo == model!.ControlNo);

            var edit = new Vermodel();
            {
                edit = model;
                edit!.Status = status!;
                edit.Result = Uresult;
                edit!.ControlNo = obj!.ControlNo!;
                edit.ActionID = obj!.ActionID;
                edit.DateVer = datever;
            }

            if (file != null)
            {
                using MemoryStream ms = new();
                file.CopyTo(ms);
                edit.Files = ms.ToArray();
            }

            if (file == null && model!.Files != null)
            {
                edit.Files = model.Files;
            }

            if (ModelState.IsValid)
            {
                _Db.VerDb.Update(edit);
                _Db.SaveChanges();
            }

            return RedirectToAction("RCVViewDet", "RCV", new { ID = obj!.ActionID });
        }
    }
}
