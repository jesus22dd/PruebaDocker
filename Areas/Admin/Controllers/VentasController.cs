using AppCompleta.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppCompleta.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class VentasController : Controller
    {
        private readonly AppCompletaContext _db;
        public VentasController(AppCompletaContext db) {
            this._db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
