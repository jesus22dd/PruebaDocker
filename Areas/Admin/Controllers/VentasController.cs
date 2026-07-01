using AppCompleta.DB;
using AppCompleta.Helpers;
using AppCompleta.Models;
using AppCompleta.ViewModels.DTOs;
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
                // No sobreescribimos TempData["Exito"] aquí, para que se vea el mensaje de Crear o Editar
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
        [HttpGet]
        public async Task<JsonResult> ObtenerClientes() {
            var clients = await _db.Clientes
                .Select(c => new
                {   c.Id,
                    c.Nombre
                }).ToListAsync();
            return Json(clients);
        }
        [HttpGet]
        public async Task<JsonResult> ObtenerProductos() {
            var products = await _db.Productos
                .Where(p => p.Stock > 0)
                .Select(p => new
                {   p.Id,
                    p.Nombre,
                    p.Precio,
                    p.Stock
                }).ToListAsync(); 
            return Json(products);
        }
        [HttpPost]
        public async Task<IActionResult> GuardarVenta([FromBody] VentaRequestDTO carrito) {
            if (carrito == null || carrito.Detalles.Count <= 0) {
                return BadRequest("No hay productos");
            }
            try
            {
                var newVenta = new Venta
                {
                    HashId = HelpMe.ReturnHashing("azxfq34?!#xfs457!45xg4850"),
                    Fecha = DateTime.Now,
                    Total = carrito.Total,
                    IdCliente = carrito.IdCliente
                };
                await _db.Venta.AddAsync(newVenta);
                await _db.SaveChangesAsync();

                foreach (var item in carrito.Detalles) {
                    var newDetalle = new DetalleVenta
                    {
                        HashId = HelpMe.ReturnHashing("azxfq34?!#xfs457!45xg4850"),
                        IdVenta = newVenta.Id,
                        IdProducto = item.IdProducto,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = item.PrecioUnitario,
                        SubTotal = item.Subtotal
                    };
                    await _db.DetalleVenta.AddAsync(newDetalle);

                    var producto = await _db.Productos.FindAsync(item.IdProducto);
                    if (producto != null) {
                        if (producto.Stock >= item.Cantidad)
                        {
                            producto.Stock -= item.Cantidad;
                        }
                        else {
                            return BadRequest("No hay stock suficiente para la transacción.");
                        }
                    }
                }
                await _db.SaveChangesAsync();

                TempData["Exito"] = "Venta registrada exitosamente.";

                return Ok(new { mensaje = "Venta registrada con exito", idVenta = newVenta.Id});
            }
            catch(Exception ex) {
                return StatusCode(500, "Error al guardar la venta" + ex.Message);
            }
        }
    }
}
