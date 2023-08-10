using Microsoft.AspNetCore.Mvc;
using PIMES_DMS.Data;
using PIMES_DMS.Models;
using System.Collections.Immutable;
using System.Diagnostics;

namespace PIMES_DMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _Db;
        private readonly List<IssueModel> mainIssues = new List<IssueModel>();
        private readonly List<ActionModel> mainActions = new List<ActionModel>();
        private readonly List<AccountsModel> mainAccounts = new List<AccountsModel>();

        public HomeController(AppDbContext db)
        {
            _Db = db;

            mainIssues = _Db.IssueDb.ToList();
            mainActions = _Db.ActionDb.ToList();
            mainAccounts = _Db.AccountsDb.ToList();
        }

        private string? GetUsername()
        {
            string username = TempData["EN"] as string;
            TempData.Keep();

            return username;
        }

        private string? GetRole()
        {
            string Role = TempData["Role"] as string;
            TempData.Keep();

            return Role;
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
                        IssueModel? issue = mainIssues.FirstOrDefault(j => j.ControlNumber == ss || j.IssueNo == ss);
                        
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
                        IssueModel? issue = mainIssues.FirstOrDefault(j => (j.ControlNumber == ss || j.IssueNo == ss) && j.ValidatedStatus);

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
                        IssueModel? issue = mainIssues.FirstOrDefault(j => j.ControlNumber == ss && j.HasTES);

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
            ViewData["InComingIssue"] = mainIssues.Where(j => !j.Acknowledged && !j.ValidatedStatus);
            ViewData["OnProgressIssue"] = mainIssues.Where(j => j.Acknowledged && !j.ValidatedStatus);

            ViewBag.Claims = GetNumberOfClaims();
            ViewBag.OnProcess = GetNumberOfOnProcess();
            ViewBag.ERA = GetNumberOfERA();
            ViewBag.RC = GetNumberOfRC();

            if (GetRole() == "CFT")
            {
                ViewBag.Acts = mainActions.Where(j => j.PIC == GetUsername()).ToList();
            }
            else if (GetRole() == "Viewer")
            {
                ViewBag.Acts =  GetCFTActionItems();
            }

            return View();
        }

        private List<ActionModel> GetCFTActionItems()
        {
            List<ActionModel> actionItems = new List<ActionModel>();

            foreach (var cft in GetCFTUnderManager())
            {
                actionItems.AddRange(mainActions.Where(j => j.PIC == cft.AccName));
            }

            return actionItems;
        }

        private List<AccountsModel> GetCFTUnderManager()
        {
            List<AccountsModel> accounts = new List<AccountsModel>();
            string Sec = mainAccounts.FirstOrDefault(j => j.AccName == GetUsername()).Section;

            switch (Sec)
            {
                case "BCManager":
                    {
                        accounts = mainAccounts.Where(j => j.Section == "BC").ToList();
                        return accounts;
                    }
                case "PurchasingManager":
                    {
                        accounts = mainAccounts.Where(j => j.Section == "Purchasing").ToList();
                        return accounts;
                    }
                case "NPIManager":
                    {
                        accounts = mainAccounts.Where(j => j.Section == "NPI").ToList();
                        return accounts;
                    }
                case "QEManager":
                    {
                        accounts = mainAccounts.Where(j => j.Section == "QE").ToList();
                        return accounts;
                    }
                case "QAManager":
                    {
                        accounts = mainAccounts.Where(j => j.Section == "QA").ToList();
                        return accounts;
                    }
                case "PPICManager":
                    {
                        accounts = mainAccounts.Where(j => j.Section == "PPIC").ToList();
                        return accounts;
                    }
                case "ProductionManager":
                    {
                        accounts = mainAccounts.Where(j => j.Section == "Production").ToList();
                        return accounts;
                    }
                case "EngineeringManager":
                    {
                        accounts = mainAccounts.Where(j => j.Section == "Engineering").ToList();
                        return accounts;
                    }
                case "VicePresident":
                    {
                        accounts = mainAccounts.ToList();
                        return accounts;
                    }
                default:
                    {
                        return accounts;
                    }
            }
        }

        private int GetNumberOfClaims()
        {
            return mainIssues.Count(j => !j.Acknowledged);
        }

        private int GetNumberOfOnProcess()
        {
            return mainIssues.Count(j => j.Acknowledged && !j.ValidatedStatus);
        }

        private int GetNumberOfERA()
        {
            return mainIssues.Count(j => (j.Acknowledged && j.ValidatedStatus && !j.HasCR && j.ValRes == "Valid")
            || (j.Acknowledged && j.ValidatedStatus && !j.HasCR && j.ValRes == "Invalid" && j.NeedRMA == "Yes"));
        }

        private int GetNumberOfRC()
        {
            List<IssueModel> issues = mainIssues.Where(j => j.HasCR && j.ValRes == "Valid").ToList();
            List<IssueModel> issuestoshow = new List<IssueModel>();

            foreach (var issue in issues)
            {
                List<ActionModel> actions = mainActions.Where(j => j.ControlNo == issue.ControlNumber).ToList();

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

            var CheckForIssueWithCN = mainIssues.FirstOrDefault(j => j.ControlNumber == controlno);

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
            IssueModel issue = mainIssues.FirstOrDefault(j => j.ControlNumber == controlno);           

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

        public IActionResult LogoutOptions(string Accs)
        {
            switch (Accs)
            {
                case "Accounts":
                    {
                        return RedirectToAction("AdminView", "Admin");
                    }
                case "Actions":
                    {
                        return RedirectToAction("ShowNotif", "Notif");
                    }
                case "Log-out":
                    {
                        return RedirectToAction("Login", "Login");
                    }
                default:
                    {
                        return Ok();
                    }
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}