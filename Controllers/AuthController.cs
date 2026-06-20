using Microsoft.AspNetCore.Mvc;
using AppCompleta.DB;
using AppCompleta.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using BCrypt.Net;
using AppCompleta.Models;
using System.Data.Common;

namespace AppCompleta.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppCompletaContext _db;
        public AuthController(AppCompletaContext db) {
            this._db = db;
        }
        [HttpGet]
        public IActionResult Index() {
            if (User.Identity != null && User.Identity.IsAuthenticated) {
                return RedirectToAction("Index","Ventas",new { area = "Admin"});
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel data) {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(data);
                }
                var usuario = await _db.Usuarios
                    .FirstOrDefaultAsync(u => u.Correo == data.Correo);

                if (usuario == null || !BCrypt.Net.BCrypt.Verify(data.Clave, usuario.Clave))
                {
                    ModelState.AddModelError(string.Empty, "Credenciales invalidas, intenta de nuevo. ");
                    return View(data);
                }

                var credencial = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name,usuario.Nombre),
                    new Claim(ClaimTypes.Email,usuario.Correo)
                };

                var identidad = new ClaimsIdentity(credencial, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidad));
                TempData["Exito"] = $"Bienvenido {usuario.Nombre}";
                return RedirectToAction("Index", "Ventas", new { area = "Admin" });
            }
            catch (Exception){
                TempData["Error"] = "Error al iniciar sesion. Verifica la conexion.";
                return RedirectToAction("Index");
            }  
        }
        [HttpGet]
        public async Task<IActionResult> Logout() {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","Home",new { area = ""});
        }

        [HttpGet]
        public IActionResult Register() {
            if (User.Identity != null && User.Identity.IsAuthenticated) {
                return RedirectToAction("Index","Ventas",new { area="Admin"});
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel data) {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(data);
                }
                bool existe = await _db.Usuarios.AnyAsync(u => u.Correo == data.Correo);
                if (existe)
                {
                    ModelState.AddModelError(string.Empty, $"{data.Correo} ya esta registrado");
                    return View(data);
                }
                string claveEncriptada = BCrypt.Net.BCrypt.HashPassword(data.Clave);

                var nuevoUsuario = new Usuario
                {
                    Nombre = data.Nombre,
                    Correo = data.Correo,
                    Clave = claveEncriptada
                };

                await _db.AddAsync(nuevoUsuario);
                await _db.SaveChangesAsync();

                TempData["Exito"] = "Usuario registrado con exito. Ya puedes iniciar sesion" + claveEncriptada;
            }
            catch (Exception){
                TempData["Error"] = "Error al registrase. Verifica la conexion.";
            }
            return RedirectToAction("Index");
        }
    }
}
