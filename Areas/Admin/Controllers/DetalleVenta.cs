using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppCompleta.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Worker")]
    public class DetalleVenta : Controller
    {
        public IActionResult Index() {
            return View();
        }
    }
}
