using AppCompleta.DB;
using AppCompleta.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppCompleta.Helpers;

namespace AppCompleta.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class ClientesController : Controller
    {
        private readonly AppCompletaContext _db;
        public ClientesController(AppCompletaContext db) {
            this._db = db;
        }
        public async Task<IActionResult> Index() {
            var clientes = await _db.Clientes.ToListAsync();
            return View(clientes);
        }
        [HttpGet]
        public IActionResult Crear() {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Cliente c) {
            ModelState.Remove(nameof(Cliente.HashId));
            if (!ModelState.IsValid) {
                TempData["Error"] = "Error al crear, verificar los datos ingresados.";
                return RedirectToAction("Index");
            }
            try {
                if (c != null) {
                    c.HashId = Helpers.HelpMe.ReturnHashing("azxfq34?!#xfs457!45xg4850");
                    c.Nombre = c.Nombre.ToUpper();
                    await _db.Clientes.AddAsync(c);
                    await _db.SaveChangesAsync();
                    TempData["Exito"] = $"Cliente {c.Nombre} registrado exitosamente";
                }
            }
            catch (Exception ex) {
                TempData["Error"] = "Error al crear. Verifica la conexion";
                throw new Exception(ex.Message);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(string code) {
            if (string.IsNullOrEmpty(code)) {
                TempData["Error"] = "Codigo invalido, error entre rutas";
                return RedirectToAction("Index");
            }
            try
            {
                var client = await _db.Clientes.FirstOrDefaultAsync(c=>c.HashId == code);
                if (client != null) { 
                    _db.Remove(client);
                    await _db.SaveChangesAsync();
                    TempData["Exito"] = $"Cliente {client.Nombre} eliminado exitosamente";
                }
            }
            catch (Exception ex){
                TempData["Error"] = "Error al eliminar. Verifica la conexion";
                throw new Exception(ex.Message);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(string code) { 
            var client = await _db.Clientes.FirstOrDefaultAsync(c => c.HashId == code);
            try {
                if (client == null)
                {
                    TempData["Error"] = "Cliente no encontrado, error entre rutas";
                    return RedirectToAction("Index");
                }
                else {
                    TempData["Exito"] = $"Registro #REG{client.Id} cargado exitosamente.";
                }
            }
            catch (Exception ex) {
                TempData["Error"] = "Error al cargar cliente. Verifica la conexion";
                throw new Exception(ex.Message);
            }
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Cliente c) {
            if (!ModelState.IsValid) {
                TempData["Error"] = "Error al editar, verificar los datos ingresados.";
                return RedirectToAction("Index");
            }
            try {
                var client = await _db.Clientes.SingleOrDefaultAsync(cl => cl.HashId == c.HashId);
                if (client != null) {
                    client.Nombre = c.Nombre;
                    client.Correo = c.Correo;
                    client.Telefono = c.Telefono;
                    await _db.SaveChangesAsync();
                    TempData["Exito"] = $"Cliente REG#{c.Id} actualizado exitosamente. ";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al editar cliente. Verifica la conexion";
                throw new Exception(ex.Message);
            }
            return RedirectToAction("Index");
        }
    }
}
