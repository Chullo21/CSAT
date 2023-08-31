using Microsoft.AspNetCore.Mvc;
using PIMES_DMS.Data;
using PIMES_DMS.Models;
using System.Net.Mail;
using System.Net;

namespace PIMES_DMS.Controllers
{
    public class IssueController : Controller
    {
        private readonly AppDbContext _context;

        public IssueController(AppDbContext context)
        {
            _context = context;
        }

        public void UpdateNotif(DateTime time, string message, string t)
        {
            string? EN = TempData["EN"] as string;
            TempData.Keep();

            NotifModel nm = new NotifModel();
            {
                nm.Message = EN + message;
                nm.DateCreated = time;
                nm.Type = t;
            }


            if (ModelState.IsValid)
            {
                _context.NotifDb.Add(nm);
            }
        }

        public IActionResult SubmitIssueView()
        {
            string? EN = TempData["EN"] as string;
            TempData.Keep();

            if (string.IsNullOrEmpty(EN))
            {
                return RedirectToAction("Logout", "Login");
            }

            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult SubmitIssue(string issueno, DateTime datefound, string product, string serialno,
            string affectedpn, string description, string problemdescription, IFormFile? cp, int qty, string fffs)
        {
            TempData["Existing8D"] = null;
            var checkForExistingIssue = _context.IssueDb.FirstOrDefault(j => j.IssueNo == issueno);

            if (checkForExistingIssue != null)
            {
                TempData["Existing8D"] = "An Issue with issue number of '" + issueno + "' already exist. Issue Submission was not successful.";
                return RedirectToAction("AdminHome", "Home");
            }

            string? creator = TempData["EN"] as string;
            TempData.Keep();
            var catchIssue = _context.IssueDb.FirstOrDefault(j => j.IssueNo == issueno);

            if (catchIssue != null)
            {
                TempData["ExistingIssue"] = "Existing";
                return View("SubmitIssueView");
            }

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
                issue.Qty = qty;
                issue.Validator = "";
                issue.FFFS = fffs;
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
                UpdateNotif(DateTime.Now, ", have submitted a claim having issue# of  " + issueno + ".", "All");

                _context.SaveChanges();

                NotifyAboutSubmittedIssue(issueno);

                return View("IssueDetails", issue);
            }

            return RedirectToAction("IssuesList");
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult ShowPdf(int ID)
        {
            var details = _context.IssueDb.Find(ID);

            byte[]? docinByte = details!.ClientRep;

            if (docinByte == null)
            {
                return NoContent();
            }
            else
            {
                return File(docinByte!, "application/pdf");
            }
        }

        [AutoValidateAntiforgeryToken]
        public IActionResult IssuesList()
        {
            string? EN = TempData["EN"] as string;
            TempData.Keep();

            if (string.IsNullOrEmpty(EN))
            {
                return RedirectToAction("Logout", "Login");
            }

            return View(_context.IssueDb.Where(j => !j.Acknowledged));
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult AcknowledgedIssues()
        {
            string? EN = TempData["EN"] as string;
            TempData.Keep();

            if (string.IsNullOrEmpty(EN))
            {
                return RedirectToAction("Logout", "Login");
            }

            return View("AckIssues", _context.IssueDb.Where(j => j.Acknowledged && !j.ValidatedStatus));
        }

        [AutoValidateAntiforgeryToken]
        public IActionResult IssueDetails(int ID)
        {
            string? EN = TempData["EN"] as string;
            TempData.Keep();

            if (string.IsNullOrEmpty(EN))
            {
                return RedirectToAction("Logout", "Login");
            }

            IssueModel? det = _context.IssueDb.Find(ID);

            return View(det);
        }

        [AutoValidateAntiforgeryToken]
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
                UpdateNotif(DateTime.Now, ", have acknowledged a claim with Issue# of " + issue!.IssueNo + ".", "All");
                _context.SaveChanges();

                return View("IssueDetails", ackissue);
            }

            return RedirectToAction("IssuesList");
        }

        public IActionResult EditIssueView(int ID)
        {
            string? EN = TempData["EN"] as string;
            TempData.Keep();

            if (string.IsNullOrEmpty(EN))
            {
                return RedirectToAction("Logout", "Login");
            }

            return View(_context.IssueDb.FirstOrDefault(j => j.IssueID == ID));
        }

        public IActionResult EditIssue(int ID, string issueno, string? serial, string? affected, int qty, string? desc, string? probdesc, IFormFile? doc)
        {

            var checkForIssueWithSameNumber = _context.IssueDb.FirstOrDefault(j => j.IssueNo == issueno && j.IssueID != ID);

            if (checkForIssueWithSameNumber != null)
            {
                TempData["Existing8D"] = "An Issue with issue number of '" + issueno + "' already exist. Request to edit Issue was not successful.";
                return RedirectToAction("AdminHome", "Home");
            }

            var issue = _context.IssueDb.FirstOrDefault(j => j.IssueID == ID);

            IssueModel model = new IssueModel();
            {
                model = issue!;
                model.IssueNo = issueno!;
                model.SerialNo = serial;
                model.AffectedPN = affected!;
                model.Qty = qty;
                model.Desc = desc!;
                model.ProbDesc = probdesc!;
            }

            if (doc != null)
            {
                using(MemoryStream ms =  new MemoryStream())
                {
                    doc.CopyTo(ms);
                    model.ClientRep = ms.ToArray();
                }
            }

            if (ModelState.IsValid)
            {
                _context.IssueDb.Update(model);
                UpdateNotif(DateTime.Now, ", have edited a claim with issue# of " + issue!.IssueNo + ".", "All");

                _context.SaveChanges();                
            }

            return RedirectToAction("IssuesList");
        }

        public IActionResult GetInvalids()
        {
            string? EN = TempData["EN"] as string;
            TempData.Keep();

            if (string.IsNullOrEmpty(EN))
            {
                return RedirectToAction("Logout", "Login");
            }

            return View(_context.IssueDb.Where(j => !j.isDeleted && j.Acknowledged && j.ValidatedStatus && j.ValRes == "Invalid"));
        }

        public IActionResult GetValids()
        {
            string? EN = TempData["EN"] as string;
            TempData.Keep();

            if (string.IsNullOrEmpty(EN))
            {
                return RedirectToAction("Logout", "Login");
            }

            return View(_context.IssueDb.Where(j => j.ValRes == "Valid"));
        }

        public IActionResult GetRMA()
        {
            string? EN = TempData["EN"] as string;
            TempData.Keep();

            if (string.IsNullOrEmpty(EN))
            {
                return RedirectToAction("Logout", "Login");
            }

            return View(_context.RMADb.OrderByDescending(j => j.DateCreated));
        }

        public IActionResult ArchiveFunct(string aval)
        {
            switch (aval)
            {
                case "Invalid":
                    {
                        return RedirectToAction("GetInvalids");
                    }
                case "Valid":
                    {
                        return RedirectToAction("GetValids");
                    }
                case "8D":
                    {
                        return RedirectToAction("Completed8D");
                    }
                case "RMA":
                    {
                        return RedirectToAction("GetRMA");
                    }
                case "CR":
                    {
                        return RedirectToAction("ERView", "ER");
                    }
                default:
                    {
                        return NoContent();
                    }
            }
        }

        public IActionResult DefinitionsView()
        {
            string? EN = TempData["EN"] as string;
            TempData.Keep();

            if (string.IsNullOrEmpty(EN))
            {
                return RedirectToAction("Logout", "Login");
            }

            return View(_context.DefDb.ToList());
        }

        public IActionResult Completed8D()
        {
            string? EN = TempData["EN"] as string;
            TempData.Keep();

            if (string.IsNullOrEmpty(EN))
            {
                return RedirectToAction("Logout", "Login");
            }

            List<IssueModel> issuestoshow = new List<IssueModel>();

            List<IssueModel> issues = _context.IssueDb.Where(j => j.HasTES).ToList();
            
            foreach (var issue in issues)
            {
                List<ActionModel> actions = _context.ActionDb.Where(j => j.ControlNo == issue.ControlNumber).ToList();

                if (actions.All(j => j.ActionStatus == "Closed" && j.VerStatus) && actions.Count > 0)
                {
                    issuestoshow.Add(issue);
                }
            }

            return View(issuestoshow);
        }

        public void NotifyAboutSubmittedIssue(string issueno)
        {
            List<string?> sendTo = _context.AccountsDb.Where(j => !string.IsNullOrEmpty(j.Email)).Select(j => j.Email).ToList();

            UserAndPass randtobeused = new UserAndPass();
            {
                Random rand = new Random();
                int random = rand.Next(0, 2);

                switch (random)
                {
                    case 0:
                        {
                            randtobeused.Email = "atsnoreply01@gmail.com";
                            randtobeused.Password = "dlthqvxnsbnfpwzs";
                            break;
                        }
                    case 1:
                        {
                            randtobeused.Email = "noreplyATS1@gmail.com";
                            randtobeused.Password = "mxmppmodmskwwzhv";
                            break;
                        }
                    case 2:
                        {
                            randtobeused.Email = "noreplyATS3@gmail.com";
                            randtobeused.Password = "peddcrnhcsswsjuf";
                            break;
                        }
                    default:
                        {
                            randtobeused.Email = "noreplyATS3@gmail.com";
                            randtobeused.Password = "peddcrnhcsswsjuf";
                            break;
                        }
                }
                //if (random == 0)
                //{
                //    randtobeused.Email = "atsnoreply01@gmail.com";
                //    randtobeused.Password = "dlthqvxnsbnfpwzs";
                //}
                //else if (random == 1)
                //{
                //    randtobeused.Email = "noreplyATS1@gmail.com";
                //    randtobeused.Password = "mxmppmodmskwwzhv";
                //}
                //else
                //{
                //    randtobeused.Email = "noreplyATS3@gmail.com";
                //    randtobeused.Password = "peddcrnhcsswsjuf";
                //}
            }

            string link = "http://192.168.3.39";

            string body = "Good day,\r\n\r\nWe have received a new quality claim from our costumer with issue# of " + issueno + ". Feel free to access our CSat Portal to review the said claim.\r\n\r\n";
            body += $"Please click \"{link}\" for your reference.\r\n\r\nHave a great day!";

            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress(randtobeused.Email);
                foreach (var to in sendTo)
                {
                    message.To.Add(to!);
                }
                message.Subject = "New Quality Issue Claim";
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

    public class UserAndPass
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

}
