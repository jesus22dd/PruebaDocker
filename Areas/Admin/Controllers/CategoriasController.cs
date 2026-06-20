using AppCompleta.DB;
using AppCompleta.Models;
using AppCompleta.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Identity.Client;

namespace AppCompleta.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoriasController : Controller
    {
        private readonly AppCompletaContext _db;
        public CategoriasController(AppCompletaContext db) {
            this._db = db;
        }
        public async Task<IActionResult> Index() { 
            var categorias = await _db.Categoria.ToListAsync();
            return View(categorias);
        }
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Categoria c) {
            ModelState.Remove(nameof(Categoria.HashId));
            if (!ModelState.IsValid) {
                TempData["Error"] = "Error al crear, verificar los datos ingresados.";
                return RedirectToAction("Index");
            }
            try {
                if (c != null) {
                    c.HashId = HelperClass.ReturnHashing("azxfq34?!#xfs457!45xg4850");
                    if (!string.IsNullOrEmpty(c.HashId))
                    {
                        await _db.Categoria.AddAsync(c);
                        await _db.SaveChangesAsync();
                        TempData["Exito"] = $"Categoria {c.Nombre} creada exitosamente";
                    }
                } 
            }
            catch (Exception) {
                TempData["Error"] = "Error al crear, verificar conexion";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(string code) {
            var cat = await _db.Categoria.FirstOrDefaultAsync(cat => cat.HashId == code);
            try
            {
                if (cat != null)
                {
                    _db.Categoria.Remove(cat);
                    await _db.SaveChangesAsync();
                    TempData["Exito"] = $"Categoria {cat.Nombre} eliminada exitosamente.";
                }
            }
            catch (Exception) {
                if (cat != null)
                    TempData["Error"] = $"Error. La categoria {cat.Nombre} tiene productos asociados ";        
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Editar(string code) {
            var cat = await _db.Categoria.FirstOrDefaultAsync(cat => cat.HashId == code);
            try
            {
                if (cat != null) {
                    TempData["Exito"] = $"Datos cargados correctamente del registro {cat.Id}";
                    return View(cat);
                }
            }
            catch {
                TempData["Error"] = "Error. Datos no encontrados";
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Categoria p) {
            if (!ModelState.IsValid) {
                TempData["Error"] = $"Error al editar, verificar los datos ingresados.";
                return RedirectToAction("Index");
            }
            try
            {
                if (p != null) {
                    var catEditar = await _db.Categoria.SingleOrDefaultAsync(cat => cat.HashId == p.HashId);
                    if (catEditar != null) {
                        catEditar.Nombre = p.Nombre;
                        await _db.SaveChangesAsync();
                        TempData["Exito"] = $"Categoria editada exitosamente.";
                    }     
                } 
            }
            catch {
                TempData["Error"] = $"Error al editar, verificar conexion.";
            }
            return RedirectToAction("Index");
        }
    }
}
