using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PIMES_DMS.Data;
using PIMES_DMS.Models;
using System.Net.Mail;
using System.Net;

namespace PIMES_DMS.Controllers
{
    public class ERController : Controller
    {
        private readonly AppDbContext _Db;

        public ERController(AppDbContext db)
        {
            _Db = db;
        }

        public void UpdateNotif(string message, string t)
        {
            string EN = TempData["EN"] as string;
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

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create_ContainmentView(int ID)
        {
            IssueModel? obj = _Db.IssueDb.FirstOrDefault(j => j.IssueID == ID);

            return View(obj);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create_Containment(string issueno, string? controlno, string? whsoh, string? whgood, string? whnogood, string? whdis, string? iqsoh, string? iqgood,
            string? iqnogood, string? iqdis, string? wisoh, string? wigood, string? winogood, string? widis, string? fgsoh, string? fggood, string? fgnogood, string? fgdis,
            bool rep)
        {
            ERModel obj = new();
            {
                obj.IssueNo = issueno;
                obj.ControlNo = controlno;
                obj.WHSESOH = whsoh;
                obj.WHSEGOOD = whgood!;
                obj.WHSENOGOOD = whnogood!;
                obj.WHSEDis = whdis!;
                obj.WIPSOH = wisoh!;
                obj.WIPGOOD = wigood!;
                obj.WIPNOGOOD = winogood!;
                obj.WIPDis = widis!;
                obj.IQASOH = iqsoh!;
                obj.IQAGOOD = iqgood!;
                obj.IQANOGOOD = iqnogood!;
                obj.IQADis = iqdis!;
                obj.FGSOH = fgsoh!;
                obj.FGGOOD = fggood!;
                obj.FGNOGOOD = fgnogood!;
                obj.FGDis = fgdis!;
                obj.Rep = rep;                
                obj.DateCreated = DateTime.Now;
                
                if (rep)
                {
                    obj.RMAno = SetRMA(issueno); 
                }
                else
                {
                    obj.RMAno = "";
                }
            }

           if (rep)
            {
                IssueModel issue = _Db.IssueDb.FirstOrDefault(j => j.IssueNo == issueno);

                var createRMA = new RMAModel();
                {
                    createRMA.DateCreated = DateTime.Now;
                    createRMA.RMANo = obj.RMAno!;
                    createRMA.IssueNo = issue.IssueNo;
                    createRMA.Product = issue.Product;                    
                    createRMA.AffectedPN = issue.AffectedPN;
                    createRMA.Description = issue.Desc;
                    createRMA.ProblemDesc = issue.ProbDesc;
                    createRMA.QTY = issue.Qty;

                    _Db.RMADb.Add(createRMA);
                }

                NotifyAboutSubmittedIssue();
            }

            if (ModelState.IsValid)
            {
                IssueModel? issue = _Db.IssueDb.FirstOrDefault(j => j.IssueNo == issueno);
                issue!.HasCR = true;

                _Db.IssueDb.Update(issue);
                _Db.ERDb.Add(obj);
                _Db.SaveChanges();

                UpdateNotif(", have submitted a new containment report.", "All");
            }

            return RedirectToAction("AdminHome", "Home");
        }

        private string SetRMA(string issueno)
        {
            DateTime dateFound = _Db.IssueDb.FirstOrDefault(j => j.IssueNo == issueno).DateFound;

            List<ERModel> er = _Db.ERDb.Where(j => !string.IsNullOrEmpty(j.RMAno) && j.DateCreated.Year.ToString().Substring(2,2) == dateFound.Year.ToString().Substring(2,2)).ToList();
            er = er.OrderByDescending(j => j.DateCreated).ToList();

            int series = 1;

            if (er.Count > 0)
            {
                series = int.Parse(er.First().RMAno.Substring(12, 3)) + 1;
            }

            return "PATS-RMA-" + dateFound.Year.ToString().Substring(2, 2) + "-" + series.ToString("000");
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult ERView()
        {
            return View(_Db.IssueDb.Where(j => j.HasCR && !j.isDeleted));
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult ERDet(string ID)
        {
            var obj = _Db.ERDb.FirstOrDefault(j => j.IssueNo == ID);

            return View("ERDet", obj);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult EditERView(string ID)
        {

            var obj = _Db.ERDb.FirstOrDefault(j => j.IssueNo == ID);

            return View(obj);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult EditER(int erid, string? controlno, string? whsoh, string? whgood, string? whnogood, string? whdis, string? iqsoh, string? iqgood,
            string? iqnogood, string? iqdis, string? wisoh, string? wigood, string? winogood, string? widis, string? fgsoh, string? fggood, string? fgnogood, string? fgdis,
            bool rep, string? rma)
        {
           
            var obj = new ERModel();
            {
                obj.ERID = erid;
                obj.ControlNo = controlno;
                obj.WHSESOH = whsoh!;
                obj.WHSEGOOD = whgood!;
                obj.WHSENOGOOD = whnogood!;
                obj.WHSEDis = whdis!;
                obj.WIPSOH = wisoh!;
                obj.WIPGOOD = wigood!;
                obj.WIPNOGOOD = winogood!;
                obj.WIPDis = widis!;
                obj.IQASOH = iqsoh!;
                obj.IQAGOOD = iqgood!;
                obj.IQANOGOOD = iqnogood!;
                obj.IQADis = iqdis!;
                obj.FGSOH = fgsoh!;
                obj.FGGOOD = fggood!;
                obj.FGNOGOOD = fgnogood!;
                obj.FGDis = fgdis!;
                obj.Rep = rep;
                obj.RMAno = rma;
            }

            _Db.ERDb.Update(obj);
            _Db.SaveChanges();

            UpdateNotif(", have edited a containment report.", "All");

            return View("ERDet", _Db.ERDb.FirstOrDefault(j => j.ControlNo == controlno));
        }

        public IActionResult DeleteView()
        {
            return View();
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult Search(string ss)
        {
            string? Role = TempData["Role"] as string;
            TempData.Keep();

            if (ss.IsNullOrEmpty())
            {
                return View("ERView", _Db.IssueDb.Where(j => j.Acknowledged && j.ValidatedStatus));
            }


            if (Role == "CLIENT")
            {

                string? EN = TempData["EN"] as string;
                TempData.Keep();

                IEnumerable<IssueModel> obj = from m in _Db.IssueDb
                                              where m.IssueCreator == EN && m.ValidatedStatus && m.Acknowledged &&
                                              (m.IssueNo.Contains(ss) || m.ControlNumber.Contains(ss))
                                              select m;

                return View("ERView", obj);
            }
            else
            {
                IEnumerable<IssueModel> obj = _Db.IssueDb.Where(m => m.ValidatedStatus && m.Acknowledged && (m.IssueCreator.Contains(ss)
                                              || m.IssueNo.Contains(ss) || m.ControlNumber.Contains(ss)));

                return View("ERView", obj);
            }
        }

        public void NotifyAboutSubmittedIssue()
        {
            List<string?> sendTo = _Db.AccountsDb.Where(j => j.Section == "BC" || j.Section == "PPIC").Select(j => j.Email).ToList();

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

            string body = "Good day,\r\n\r\nA new RMA number has been generated. You can view this data by visiting our CSat Portal.\r\n\r\nHave a great day!\r\n\r\n";
            body += $"Please click \"{link}\" for your reference.";

            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress(randtobeused.Email);
                foreach (var to in sendTo)
                {
                    message.To.Add(to);
                }
                message.Subject = "New RMA Generated";
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
