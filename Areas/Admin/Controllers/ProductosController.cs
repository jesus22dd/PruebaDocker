using AppCompleta.DB;
using AppCompleta.Helpers;
using AppCompleta.Models;
using AppCompleta.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AppCompleta.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly AppCompletaContext _db;
        public ProductosController(AppCompletaContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index() { 
            var productos = await _db.Productos
                .Include(p => p.IdCategoriaNavigation)
                .ToListAsync();
            return View(productos);
        }
        public async Task<IActionResult> Crear() {
            var productos = new ProductosViewModel
            {
                Productos = new Producto(),
                Categorias = await _db.Categoria.ToListAsync()
            };
            return View(productos);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(ProductosViewModel p) {
            ModelState.Remove("Productos.HashId");
            ModelState.Remove("Productos.IdCategoriaNavigation");
            if (!ModelState.IsValid) {
                TempData["Error"] = "Error al crear, verificar los datos ingresados.";
                return RedirectToAction("Index");
            }
            try
            {
                if (p.Productos != null) {
                    p.Productos.HashId = HelperClass.ReturnHashing("azxfq34?!#xfs457!45xg4850");
                    if (!string.IsNullOrEmpty(p.Productos.HashId))
                    {
                        await _db.AddAsync(p.Productos);
                        await _db.SaveChangesAsync();
                        TempData["Exito"] = $"Producto {p.Productos.Nombre} creado exitosamente.";
                    }
                }
            }
            catch {
                TempData["Error"] = "Error al crear, verificar conexion";
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(string code) {
            try
            {
                var p = await _db.Productos.FirstOrDefaultAsync(prod => prod.HashId == code);
                if (p != null) {
                    _db.Productos.Remove(p);
                    await _db.SaveChangesAsync();
                    TempData["Exito"] = $"Producto {p.Nombre} eliminado exitosamente.";
                }
            }
            catch {
                TempData["Error"] = "Error al eliminar producto. Verifica tu conexion.";
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Editar(string code)
        {
            try {
                var p = await _db.Productos.FirstOrDefaultAsync(prod => prod.HashId == code);
                if (p != null) {
                    var productos = new ProductosViewModel
                    {
                        Productos = p,
                        Categorias = await _db.Categoria.ToListAsync()
                    };
                    TempData["Exito"] = $"Datos cargados correctamente del registro {p.Id}";
                    return View(productos);
                }
            } catch {
                TempData["Error"] = "Error. Datos no encontrados";
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(ProductosViewModel pd)
        {
            ModelState.Remove("Productos.idCategoriaNavigation");
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Error en los datos ingresados.";
                return RedirectToAction("Index");
            }
            try
            {
                if (pd.Productos != null)
                {
                    var prodEditar = await _db.Productos
                    .Where(p => p.HashId == pd.Productos.HashId)
                    .ExecuteUpdateAsync(s => s
                        .SetProperty(p => p.Nombre, pd.Productos.Nombre)
                        .SetProperty(p => p.Detalle, pd.Productos.Detalle)
                        .SetProperty(p => p.Precio, pd.Productos.Precio)
                        .SetProperty(p => p.Stock, pd.Productos.Stock)
                        .SetProperty(p => p.IdCategoria, pd.Productos.IdCategoria)
                    );
                    TempData["Exito"] = "Producto actualizado exitosamente";
                }
            }
            catch
            {
                TempData["Error"] = "Error al editar. Verificar conexion";
            }
            return RedirectToAction("Index");
        }
    }
}
