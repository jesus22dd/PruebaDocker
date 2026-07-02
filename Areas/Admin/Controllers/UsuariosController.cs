using AppCompleta.DB;
using AppCompleta.Models;
using AppCompleta.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Claims;

namespace AppCompleta.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles="Admin")]
    public class UsuariosController : Controller
    {
        private readonly AppCompletaContext _db;
        public UsuariosController(AppCompletaContext db) {
            this._db = db;
        }
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
        {
            try
            {
                ViewData["CurrentSort"] = sortOrder;
                ViewData["CurrentFilter"] = searchString;

                var usuariosQuery = _db.Usuarios.AsNoTracking();

                if (!string.IsNullOrEmpty(searchString))
                {
                    var searchLower = searchString.ToLower();
                    usuariosQuery = usuariosQuery.Where(u => 
                        u.Nombre.ToLower().Contains(searchLower) || 
                        u.Correo.ToLower().Contains(searchLower));
                }

                if (sortOrder == "antiguo")
                {
                    usuariosQuery = usuariosQuery.OrderBy(u => u.Id);
                }
                else
                {
                    usuariosQuery = usuariosQuery.OrderByDescending(u => u.Id);
                }

                int pageSize = 8;
                return View(await PaginatedList<Usuario>.CreateAsync(usuariosQuery, pageNumber ?? 1, pageSize));
            }
            catch
            {
                TempData["Error"] = "Error al cargar los datos. Verifica tu conexion.";
            }
            return View();
        }
        public IActionResult Crear() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Crear(Usuario user) {
            try
            {
                bool existe = await _db.Usuarios.AnyAsync(u => u.Correo == user.Correo);
                if (existe)
                {
                    ModelState.AddModelError(string.Empty, $"{user.Correo} ya esta registrado");
                    return View(user);
                }
                
                string newClave = HelpMe.ReturnNewPwd(user.Nombre,user.Correo);
                string claveEncriptada = BCrypt.Net.BCrypt.HashPassword(newClave);
                user.Clave = claveEncriptada;

                await _db.Usuarios.AddAsync(user);
                await _db.SaveChangesAsync();
                TempData["Exito"] = $"Usuario {user.Nombre} creado correctamente con Contraseña: {newClave}";
                return RedirectToAction("Index");
            }
            catch {
                TempData["Error"] = "Error al crear. Verifica tu conexion";
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id) {
            try
            {
                if (User.Identity!.IsAuthenticated) {
                    int same = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                    if (same == id)
                    {
                        TempData["Error"] = "No puedes eliminar una cuenta autenticada.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        var user = await _db.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
                        if (user != null)
                        {
                            if (user.Id == 1) {
                                TempData["Error"] = "No puedes eliminar al SuperAdministrador";
                                return RedirectToAction("Index");
                            }
                            _db.Remove(user);
                            await _db.SaveChangesAsync();
                            TempData["Exito"] = $"Usuario {user.Nombre} eliminado correctamente.";
                            return RedirectToAction("Index");
                        }
                    }
                } 
            }
            catch {
                TempData["Error"] = "Error al eliminar. Verifica tu conexion.";
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Detalle(int id) {
            try
            {
                var user = await _db.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
                if (user != null)
                {
                    TempData["Exito"] = $"Datos cargados correctamente del registro {user.Id}";
                    return View(user);
                }
            }
            catch {
                TempData["Error"] = "Error al cargar los datos. Verifica tu conexion.";
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Restablecer(int id)
        {
            try
            {
                bool exists = await _db.Usuarios.AnyAsync(u => u.Id == id);
                if (exists) {
                    var user = await _db.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
                    if (user != null) {
                        if (user.Id == 1)
                        {
                            TempData["Error"] = "No puedes restablecer al SuperAdministrador";
                            return RedirectToAction("Index");
                        }
                        string newClave = HelpMe.ReturnNewPwd(user.Nombre, user.Correo);
                        string claveEncriptada = BCrypt.Net.BCrypt.HashPassword(newClave);
                        var userRest = await _db.Usuarios
                            .Where(u => u.Id == id)
                            .ExecuteUpdateAsync(p => p
                                .SetProperty(sp => sp.Clave, claveEncriptada));
                        await _db.SaveChangesAsync();

                        TempData["Exito"] = $"Contraseña del usuario {user.Nombre} restablecida correctamente a: {newClave}";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch {
                TempData["Error"] = "Error al restablecer la contraseña. Verifica tu conexion.";
            }
            return RedirectToAction("Index");
        }
    }
}
