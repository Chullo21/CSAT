using Microsoft.AspNetCore.Mvc;
using PIMES_DMS.Data;
using PIMES_DMS.Models;

namespace PIMES_DMS.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _Db;

        public AdminController(AppDbContext db)
        {
            _Db = db;
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult AdminView()
        {
            return View(_Db.AccountsDb);
        }

        public IActionResult CreateUserView()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult CreateUser(string username, string password, string accname, string compname, string role)
        {

            if (username != null || password != null || accname != null || compname != null || role != null)
            {
                Guid guid = Guid.NewGuid();

                AccountsModel acc = new();
                {
                    acc.AccUCode = role + "-" + compname + "-" + guid;
                    acc.AccName = accname!;
                    acc.Role = role;
                    acc.CompName = compname!;
                    acc.UserName = username!;
                    acc.Password = password!;
                }

                if (ModelState.IsValid)
                {
                    _Db.AccountsDb.Add(acc);
                    _Db.SaveChanges();
                }
            }
           

            return RedirectToAction("AdminView");
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult EditView(int ID)
        {

            if (ID.ToString() == null)
            {
                return NotFound();
            }

            AccountsModel? findEdit = _Db.AccountsDb.Find(ID);

            if (findEdit == null)
            {
                return NotFound();
            }

            return View(findEdit);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult EditUser(AccountsModel obj)
        {

            if (ModelState.IsValid)
            {
                _Db.AccountsDb.Update(obj);
                _Db.SaveChanges();
            }

            return RedirectToAction("AdminView");
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult Details(int ID)
        {
            var det = _Db.AccountsDb.Find(ID);

            if (det != null)
            {
                return View(det);
            }
            
            return NotFound();
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult DeleteView(int ID)
        {
            var del = _Db.AccountsDb.Find(ID);

            return View(del);
        }

        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(int ID)
        {

            var del = _Db.AccountsDb.FirstOrDefault(j => j.AccID == ID);

            _Db.AccountsDb.Remove(del);
            _Db.SaveChanges();

            return RedirectToAction("AdminView");
        }

    }
}
