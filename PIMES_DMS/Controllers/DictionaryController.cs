using Microsoft.AspNetCore.Mvc;
using PIMES_DMS.Data;
using PIMES_DMS.Models;

namespace PIMES_DMS.Controllers
{
    public class DictionaryController : Controller
    {
        private readonly AppDbContext _Db;

        public DictionaryController(AppDbContext Db)
        {
            _Db = Db;
        }

        public IActionResult AddTerm(string subj, string def)
        {
            DefinitionsModel model = new DefinitionsModel();
            {
                model.Subject = subj;
                model.Definition = def;
            }

            if (ModelState.IsValid)
            {
                _Db.DefDb.Add(model);
                _Db.SaveChanges();
            }

            return RedirectToAction("DefinitionsView", "Issue");
        }

        public IActionResult EditTerm(int defid, string subj, string def)
        {
            DefinitionsModel model = new DefinitionsModel();
            {
                model.DefID = defid;
                model.Subject = subj;
                model.Definition = def;

            }

            if (ModelState.IsValid)
            {
                _Db.DefDb.Update(model);
                _Db.SaveChanges();
            }

            return RedirectToAction("DefinitionsView", "Issue");
        }

        public IActionResult DeleteTerm(int defid, string subj, string def)
        {
            DefinitionsModel model = new DefinitionsModel();
            {
                model.DefID = defid;
                model.Subject = subj;
                model.Definition = def;

            }

            if (ModelState.IsValid)
            {
                _Db.DefDb.Remove(model);
                _Db.SaveChanges();
            }

            return RedirectToAction("DefinitionsView", "Issue");
        }
    }
}
