using AppCompleta.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppCompleta.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Worker")]
    public class VentasController : Controller
    {
        private readonly AppCompletaContext _db;
        public VentasController(AppCompletaContext db) {
            this._db = db;
        }
        public async Task<IActionResult> Index()
        {
            try {
                var ventas = await _db.Venta
                    .Include(p => p.IdClienteNavigation)
                    .ToListAsync();
                await _db.SaveChangesAsync();
                TempData["Exito"] = "Ventas cargadas correctamente.";
                return View(ventas);
            } catch(Exception ex) {
                TempData["Error"] = "Error al cargar ventas. Verifica tu conexion." + ex.Message;
            }
            return View();
        }
        public IActionResult Crear() {
            return View();
        }
        public IActionResult Anular() {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Detalle(string code) {
            try {
                var venta = await _db.Venta
                    .Include(v => v.IdClienteNavigation)
                    .FirstOrDefaultAsync(v => v.HashId == code);
                if (venta != null) {
                    TempData["Exito"] = $"Venta Nro {venta.Id} cargado correctamente.";
                    return View(venta);
                }
            }
            catch (Exception ex){
                TempData["Error"] = "Error al cargar la venta. Verifica tu conexion" + ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}
