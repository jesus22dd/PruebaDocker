using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppCompleta.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Worker")]
    public class DetalleVentaController : Controller
    {
        public IActionResult Index() {
            return View();
        }
    }
}
