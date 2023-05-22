using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PIMES_DMS.Data;
using PIMES_DMS.Models;

namespace PIMES_DMS.Controllers
{
    public class ERController : Controller
    {
        private readonly AppDbContext _Db;

        public ERController(AppDbContext db)
        {
            _Db = db;
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
        public IActionResult Create_Containment(string? controlno, string? whsoh, string? whgood, string? whnogood, string? whdis, string? iqsoh, string? iqgood,
            string? iqnogood, string? iqdis, string? wisoh, string? wigood, string? winogood, string? widis, string? fgsoh, string? fggood, string? fgnogood, string? fgdis,
            bool rep, string? rma)
        {
            ERModel obj = new();
            {
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
                obj.RMAno = rma;
                obj.DateCreated = DateTime.Now;
            }

            if (ModelState.IsValid)
            {
                IssueModel? issue = _Db.IssueDb.FirstOrDefault(j => j.ControlNumber == controlno);
                issue!.HasCR = true;


                _Db.IssueDb.Update(issue);               
                _Db.ERDb.Add(obj);
                _Db.SaveChanges();
            }

            return RedirectToAction("AdminHome", "Home");
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult ERView()
        {
            string? Role = TempData["Role"] as string;
            TempData.Keep();

            if (Role == "CLIENT")
            {
                string? EN = TempData["EN"] as string;
                TempData.Keep();

                IEnumerable<IssueModel> objs = _Db.IssueDb.Where(j => j.HasCR && !j.isDeleted && j.IssueCreator == EN);

                return View(objs);
            }
            else
            {
                IEnumerable<IssueModel> objs = _Db.IssueDb.Where(j => j.HasCR && !j.isDeleted);
                
                return View(objs);
            }           
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult ERDet(string ID)
        {
            var obj = _Db.ERDb.FirstOrDefault(j => j.ControlNo == ID);

            return View("ERDet", obj);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult EditERView(string ID)
        {

            var obj = _Db.ERDb.FirstOrDefault(j => j.ControlNo == ID);

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


            return View("ERDet", _Db.ERDb.FirstOrDefault(j => j.ControlNo == controlno));
        }

        public IActionResult DeleteView()
        {
            return View();
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult ClientListER()
        {
            string? Role = TempData["Role"] as string;
            TempData.Keep();

            if (Role == "CLIENT")
            {
                string? EN = TempData["EN"] as string;
                TempData.Keep();

                IEnumerable<IssueModel> objs = _Db.IssueDb.Where(j => j.HasCR && !j.isDeleted && j.IssueCreator == EN);

                return View(objs);
            }
            else
            {
                IEnumerable<IssueModel> objs = _Db.IssueDb.Where(j => j.HasCR && !j.isDeleted);

                return View(objs);
            }
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
    }
}
