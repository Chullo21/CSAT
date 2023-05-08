using Microsoft.AspNetCore.Mvc;
using PIMES_DMS.Data;

namespace PIMES_DMS.Controllers
{
    public class RCVController : Controller
    {
        private readonly AppDbContext _Db;

        public RCVController(AppDbContext db)
        {
            _Db = db;
        }

        public IActionResult RCVView()
        {
            return View();
        }
    }
}
