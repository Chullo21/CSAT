using Microsoft.AspNetCore.Mvc;
using PIMES_DMS.Data;
using PIMES_DMS.Models;
using System.Net.Mail;
using System.Net;

namespace PIMES_DMS.Controllers
{
    public class RCVController : Controller
    {
        private readonly AppDbContext _Db;
        private readonly List<IssueModel> mainIssues = new List<IssueModel>();
        private readonly List<ActionModel> mainActions = new List<ActionModel>();
        private readonly List<Vermodel> mainVers = new List<Vermodel>();

        public RCVController(AppDbContext db)
        {
            _Db = db;
            mainIssues = _Db.IssueDb.ToList();
            mainActions = _Db.ActionDb.ToList();
            mainVers = _Db.VerDb.ToList();
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

        public IActionResult RecommitActionItem(int actID, DateTime date)
        {
            ActionModel action = mainActions.FirstOrDefault(j => j.ActionID == actID);

            ActionModel act = new ActionModel();
            {
                act = action;
                act.TargetDate = date;
                act.VerStatus = false;
            }

            TargetDateModel targetDateModel = new TargetDateModel();
            {
                targetDateModel.ActionID = actID;
                targetDateModel.ControlNo = act.ControlNo;
                targetDateModel.TD = date;
                targetDateModel.Status = "Open";
            }

            if (ModelState.IsValid)
            {
                _Db.ActionDb.Update(act);
                _Db.TDDb.Add(targetDateModel);
                _Db.SaveChanges();
            }

            return RedirectToAction("TESActions", new {ID = act.ControlNo});
        }

        public IActionResult VerifyActionItem(int actionid, string status, IFormFile file, string remarks, DateTime date)
        {
            ActionModel action = mainActions.FirstOrDefault(j => j.ActionID == actionid);

            ActionModel act = new ActionModel();
            {
                act = action;
                act.ActionStatus = status;
                act.VerRemarks = remarks;
                act.DateVerified = date;
                act.VerStatus = true;
                using(MemoryStream ms =  new MemoryStream())
                {
                    file.CopyTo(ms);
                    act.VerificationFile = ms.ToArray();
                }
            }

            if (ModelState.IsValid)
            {
                _Db.ActionDb.Update(act);
                _Db.SaveChanges();
            }

            return RedirectToAction("RCVViewDet", new {ID = actionid});
        }

        public IActionResult ShowPdf(string ID)
        {
            var rep = _Db._8DDb.FirstOrDefault(j => j.ControlNo == ID);

            return File(rep.Report, "application/pdf");
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult RCVList()
        {
            string? EN = TempData["EN"] as string;
            TempData.Keep();

            if (string.IsNullOrEmpty(EN))
            {
                return RedirectToAction("Logout", "Login");
            }

            GetDataForVerification();
            GetOpenAndCloseDataForTable();

            List<IssueModel> issues = mainIssues.Where(j => j.HasCR && j.ValRes == "Valid" && !j.Verified).ToList();
            List<IssueModel> issuestoshow = new List<IssueModel>();

            foreach (var issue in issues)
            {
                List<ActionModel> actions = mainActions.Where(j => j.ControlNo == issue.ControlNumber).ToList();

                if (!actions.All(j => j.ActionStatus == "Closed" && j.VerStatus) || actions.Count == 0)
                {
                    issuestoshow.Add(issue);
                }
            }

            return View(issuestoshow.OrderByDescending(j => j.DateFound).ToList());
        }

        public void GetOpenAndCloseDataForTable()
        {
            ViewData["openandclosed"] = null;

            List<IssueModel> validIssues = mainIssues.Where(j => !j.isDeleted && j.ValRes == "Valid" && j.HasCR).ToList();
            List<OpenAndClosed> oac = new List<OpenAndClosed>();

            foreach (var issue in validIssues)
            {
                int open = mainActions.Count(j => j.ControlNo == issue.ControlNumber && j.ActionStatus == "Open");

                int closed = mainActions.Count(j => j.ControlNo == issue.ControlNumber && j.ActionStatus == "Closed");

                OpenAndClosed oacc = new OpenAndClosed();
                {
                    oacc.ControlNo = issue.ControlNumber;   
                    oacc.Open = open;
                    oacc.Closed = closed;
                }

                oac.Add(oacc);
            }

            ViewData["openandclosed"] = oac;
        }

        public void GetDataForVerification()
        {
            ViewData["ForVers"] = null;

            List<IssueModel> issues = mainIssues.Where(j => j.ValRes == "Valid" && j.HasTES).ToList();

            List<ForVerificationData> fvd = new List<ForVerificationData>();

            foreach (var issue in issues)
            {
                int tc = mainActions.Count(j => (!j.VerStatus && j.TargetDate < DateTime.Now.Date && j.ControlNo == issue.ControlNumber) || (!j.VerStatus && j.ActionStatus == "Closed" && j.ControlNo == issue.ControlNumber));

                ForVerificationData fvdd = new ForVerificationData();
                {
                    fvdd.ControlNo = issue.ControlNumber;
                    fvdd.ForVer = tc;
                }

                fvd.Add(fvdd);
            }

            ViewData["ForVers"] = fvd;
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult RCVView(int ID)
        {
            string? EN = TempData["EN"] as string;
            TempData.Keep();

            if (string.IsNullOrEmpty(EN))
            {
                return RedirectToAction("Logout", "Login");
            }

            return View(mainIssues.FirstOrDefault(j => j.IssueID == ID));
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult ClientViewRCV()
        {
            string? EN = TempData["EN"] as string;
            TempData.Keep();

            if (string.IsNullOrEmpty(EN))
            {
                return RedirectToAction("Logout", "Login");
            }

            string? Role = TempData["Role"] as string;
            TempData.Keep();

            if (Role == "CLIENT")
            {
                return View(mainIssues.Where(j => j.Acknowledged && j.ValidatedStatus && j.HasCR && j.IssueCreator == EN && !j.isDeleted && j.HasTES));
            }
            else
            {
                return View(mainIssues.Where(j => j.Acknowledged && j.ValidatedStatus && j.HasCR && !j.isDeleted && j.HasTES));
            }
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult ViewVer(int ID)
        {
            string? EN = TempData["EN"] as string;
            TempData.Keep();

            if (string.IsNullOrEmpty(EN))
            {
                return RedirectToAction("Logout", "Login");
            }

            var action = mainActions.FirstOrDefault(j => j.ActionID == ID);
            List<ShowVerification> sv = new List<ShowVerification>();
            var verifications = mainVers.Where(j => !j.IsDeleted && j.RCType == "tc" && j.ActionID == ID);

            foreach (var ver in verifications)
            {
                ShowVerification svv = new ShowVerification();
                {
                    svv.ControlNo = ver.ControlNo;
                    svv.Result = ver.Result;
                    svv.DateVerified = ver.DateVer;
                    svv.Verificator = ver.Verificator; ;
                    svv.Status = ver.Status;

                    if (ver.Status == "c") svv.DateClosed = ver.StatusDate;
                }

                sv.Add(svv);
            }

            ViewData["Vers"] = sv;

            return View("VerRCVView", action);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult VerRCV(int id, string? result, DateTime date, IFormFile? evidence, string status, DateTime statusDate, string type, string controlno)
        {

            if (statusDate < DateTime.Now.Date)
            {
                TempData["WrongDate"] = "WrongDate";

                return NoContent();
            }

            string? user = TempData["EN"] as string;
            TempData.Keep();

            Vermodel model = new Vermodel();
            {
                model.RCType = type;
                model.DateVer = date;
                model.StatusDate = statusDate;
                model.Result = result;
                model.Status = status;
                model.Verificator = user;
                model.ActionID = id;
                model.ControlNo = controlno;
                model.DateVer = DateTime.Now;
            }

            if (evidence != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    evidence.CopyTo(ms);
                    model.Files = ms.ToArray();
                }   
            }

            var am = mainActions.FirstOrDefault(j => j.ActionID == id);

            ActionModel newTc = new ActionModel();
            {
                newTc = am!;
                newTc.HasVer = true;
                newTc.ActionStatus = status;
                newTc.TargetDate = statusDate;
            }

            if (status == "Open")
            {
                TargetDateModel targetDateModel = new TargetDateModel();
                {
                    targetDateModel.ActionID = am!.ActionID!;
                    targetDateModel.ControlNo = controlno;
                    targetDateModel.TD = statusDate;
                    targetDateModel.Status = status;
                }

                _Db.TDDb.Add(targetDateModel);
            }

            if (ModelState.IsValid)
            {
                _Db.ActionDb.Update(newTc);
                _Db.VerDb.Add(model);
                UpdateNotif(", have verified an action.", "All");
                _Db.SaveChanges();              
            }

            return RedirectToAction("TESActions", new { ID= controlno });
        }

        public void CheckForArt(string contno)
        {
            IssueModel? issue = mainIssues.FirstOrDefault(j => j.ControlNumber == contno);

            if (_Db.ART_8D.Count(j => j.ControlNo == contno) == 0)
            {               
                ART_8DModel art = new ART_8DModel();
                {
                    art.ControlNo = contno;
                    art.DateValidated = issue!.DateVdal!;
                    art.DateClosed = DateTime.Now;
                }

                if (ModelState.IsValid)
                {
                    _Db.ART_8D.Add(art);
                    _Db.SaveChanges();
                }
            }
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult RCVViewDet(int ID)
        {
            string? EN = TempData["EN"] as string;
            TempData.Keep();

            if (string.IsNullOrEmpty(EN))
            {
                return RedirectToAction("Logout", "Login");
            }

            ViewBag.td = null;
            ViewData["Ver"] = null;

            var action = mainActions.FirstOrDefault(j => j.ActionID == ID && !j.IsDeleted);

            ViewBag.td = _Db.TDDb.Where(j => j.ActionID == ID).OrderByDescending(j => j.TD).First();

            if (action == null)
            {
                return NotFound();
            }

            ViewData["Action"] = action;
            ViewData["Ver"] = mainVers.Where(i => i.ActionID == ID);

            return View();
        }

        public IActionResult ShowVerPdf(int ID)
        {
            byte[]? file = mainActions.FirstOrDefault(j => j.ActionID == ID).VerificationFile;

            if (file == null || file.Count() <= 0)
            {
                return NoContent();
            }

            return File(file, "application/pdf");
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult ShowFile(int ID)   
        {
            byte[]? file = mainVers.FirstOrDefault(j => j.VerID == ID).Files;

            if (file == null || file.Count() <= 0)
            {
                return NoContent();
            }

            return File(file, "application/pdf");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult UpdateVER(int ID, DateTime datever, string? status, string? Uresult, IFormFile? file, DateTime statusDate)
        {
            var model = mainVers.FirstOrDefault(m => m.VerID == ID);
            var obj = mainActions.FirstOrDefault(j => j.ControlNo == model!.ControlNo);

            var edit = new Vermodel();
            {
                edit = model;
                edit!.Status = status!;
                edit.Result = Uresult;
                edit.DateVer = datever;
                edit.StatusDate = statusDate;
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
                UpdateNotif(", have edited a verification report.", "All");
                _Db.SaveChanges();               
            }

            return RedirectToAction("RCVViewDet", "RCV", new { ID = model!.ActionID });
        }

        public IActionResult CreateTESView(int ID)
        {
            string? EN = TempData["EN"] as string;
            TempData.Keep();

            if (string.IsNullOrEmpty(EN))
            {
                return RedirectToAction("Logout", "Login");
            }

            return View(mainIssues.FirstOrDefault(j => j.IssueID == ID));
        }

        [HttpPost]
        public IActionResult CreateTES(string ID ,List<string>? twhy, List<string>? ewhy, List<string>? swhy)
        {
            TESModel tes = new TESModel();
            {
                tes.ControlNo = ID;
                tes.TCWhy1 = twhy?[0];
                tes.TCWhy2 = twhy?[1];
                tes.TCWhy3 = twhy?[2];
                tes.TCWhy4 = twhy?[3];
                tes.TRC = twhy.Last(j => !string.IsNullOrEmpty(j));
                tes.ECWhy1 = ewhy?[0];
                tes.ECWhy2 = ewhy?[1];
                tes.ECWhy3 = ewhy?[2];
                tes.ECWhy4 = ewhy?[3];
                tes.ERC = ewhy.Last(j => !string.IsNullOrEmpty(j));
                tes.SCWhy1 = swhy?[0];
                tes.SCWhy2 = swhy?[1];
                tes.SCWhy3 = swhy?[2];
                tes.SCWhy4 = swhy?[3];
                tes.SRC = swhy.Last(j => !string.IsNullOrEmpty(j));
            }

            var issue = mainIssues.FirstOrDefault(j => j.ControlNumber == ID);

            IssueModel updateIssue = new IssueModel();
            {
                updateIssue = issue!;
                updateIssue!.HasTES = true;
            }

            if (ModelState.IsValid)
            {
                _Db.IssueDb.Update(updateIssue);
                _Db.TESDb.Add(tes);
                UpdateNotif(", have submitted a 3x5why with Control# of " + tes.ControlNo + ".", "All");
                _Db.SaveChanges();
            }
            else
            {
                TempData["errormsg"] = "An error has occured, please contact an admin";
            }

            return RedirectToAction("TESActions",new {ID = ID});
        }

        public IActionResult TESActions(string ID)
        {
            string? EN = TempData["EN"] as string;
            TempData.Keep();

            if (string.IsNullOrEmpty(EN))
            {
                return RedirectToAction("Logout", "Login");
            }

            ViewBag.tds = null;
            List<Vermodel> vermodels = new List<Vermodel>();
            List<ActionModel> actions = mainActions.Where(j => !j.IsDeleted && j.ControlNo == ID).ToList();

            ViewData["actions"] = actions;
            ViewBag.tds = _Db.TDDb.Where(j => j.ControlNo == ID).ToList();
            ViewBag.pic = _Db.AccountsDb.Select(account => account.AccName).ToList();

            foreach (var action in actions)
              {
                if (action.HasVer)
                {
                    Vermodel vermodel = mainVers.Where(j => j.ActionID == action.ActionID).OrderByDescending(j => j.DateVer).First();
                    vermodels.Add(vermodel);
                }
            }

            ViewBag.remarks = vermodels;

            ViewBag._8D = _Db._8DDb.Any(j => j.ControlNo == ID);

            return View(_Db.TESDb.FirstOrDefault(j => j.ControlNo == ID));
        }

        public IActionResult SubmitActionItem(string ID, string action, string pic, DateTime td, string whichdb, string dep)
        {
            CheckForArt(ID);

            ActionModel act = new ActionModel();
            {
                act.ControlNo = ID;
                act.Action = action;
                act.PIC = pic;
                act.TargetDate = td;
                act.ActionStatus = "Open";
                act.Type = whichdb;
                act.Dependency = dep;
            }

            if (ModelState.IsValid)
            {
                NotifyAboutSubmittedIssue(pic, ID);

                _Db.ActionDb.Add(act);
                UpdateNotif(", have submitted an action item on a CAPA with Control# of " + act.ControlNo + ".", "All");
                _Db.SaveChanges();
                
                AddTd(ID, action, whichdb, td);
            }

            return RedirectToAction("TESActions", new { ID });
        }

        void AddTd(string ID, string action, string whichdb, DateTime td)
        {
            var setTd = _Db.ActionDb.FirstOrDefault(j => j.ControlNo == ID && j.Action == action && j.Type == whichdb);

            TargetDateModel targetDateModel = new TargetDateModel();
            {
                targetDateModel.ActionID = setTd.ActionID;
                targetDateModel.ControlNo = ID;
                targetDateModel.TD = td;
                targetDateModel.Status = "Open";
            }

            if (ModelState.IsValid)
            {
                _Db.TDDb.Add(targetDateModel);
                _Db.SaveChanges();
            }
        }

        public void NotifyAboutSubmittedIssue(string pic, string controlno)
        {
            var sendTo = _Db.AccountsDb.FirstOrDefault(j => j.AccName == pic && !string.IsNullOrEmpty(j.Email));

            if (sendTo != null && !string.IsNullOrEmpty(sendTo.Email))
            {
                UserAndPass randtobeused = new UserAndPass();
                {
                    Random rand = new Random();
                    int random = rand.Next(0, 2);

                    if (random == 0)
                    {
                        randtobeused.Email = "atsnoreply01@gmail.com";
                        randtobeused.Password = "dlthqvxnsbnfpwzs";
                    }
                    else if (random == 1)
                    {
                        randtobeused.Email = "noreplyATS1@gmail.com";
                        randtobeused.Password = "mxmppmodmskwwzhv";
                    }
                    else
                    {
                        randtobeused.Email = "noreplyATS3@gmail.com";
                        randtobeused.Password = "peddcrnhcsswsjuf";
                    }
                }

                string link = "http://192.168.3.39";

                string body = "Good day,\r\n\r\nYou have been asigned as PIC to an action item, having the controller number of " + controlno
                    + ". You can view this data by visiting our CSat Portal.\r\n\r\n";
                body += $"Please click \"{link}\" for your reference.\r\n\r\nHave a great day!";

                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress(randtobeused.Email);
                    message.To.Add(sendTo.Email);
                    message.Subject = "New Action Item";
                    message.Body = body;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com"))
                    {
                        smtp.Port = 587;
                        smtp.Credentials = new NetworkCredential(randtobeused.Email, randtobeused.Password);
                        smtp.EnableSsl = true;
                        smtp.Timeout = 10000;
                        if (message.To.Count() > 0)
                        {
                            smtp.Send(message);
                        }
                    }
                }
            }
        }
    }

    class NoVers
    {
        public string? ControlNo { get; set; }
    }

    class OpenAndClosed
    {
        public string? ControlNo { get; set; }
        public int Open { get; set; }
        public int Closed { get; set; }
    }

    class ForVerificationData
    {
        public string? ControlNo { get; set; }
        public int ForVer { get; set; }
    }

    class ShowVerification
    {
        public string? ControlNo { get; set; }
        public string? Result { get; set; }
        public DateTime DateVerified { get; set; }
        public string? Status { get; set; }
        public DateTime? DateClosed { get; set; }
        public string? Verificator { get; set; }
    }
}
