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
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber) {
            // Guardar el estado de los filtros para mantenerlos al navegar por la paginación
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = searchString;

            // Inicializar la consulta base
            var clientes = from c in _db.Clientes select c;

            // Búsqueda (Filtro por Nombre, Correo o Teléfono)
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                clientes = clientes.Where(c => c.Nombre.ToLower().Contains(searchString) || 
                                               (c.Correo != null && c.Correo.ToLower().Contains(searchString)) ||
                                               (c.Telefono != null && c.Telefono.ToLower().Contains(searchString)));
            }

            // Ordenamiento (Por defecto recientes primero, es decir, Id mayor)
            if (sortOrder == "antiguo")
            {
                clientes = clientes.OrderBy(c => c.Id);
            }
            else
            {
                clientes = clientes.OrderByDescending(c => c.Id);
            }

            // Paginación (8 registros por página)
            int pageSize = 8;
            return View(await PaginatedList<Cliente>.CreateAsync(clientes.AsNoTracking(), pageNumber ?? 1, pageSize));
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
