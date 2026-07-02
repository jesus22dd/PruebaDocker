using AppCompleta.DB;
using AppCompleta.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace AppCompleta.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly AppCompletaContext _db;

        public DashboardController(AppCompletaContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new DashboardViewModel();
            var hoy = DateTime.Now.Date;
            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);
            var hace7Dias = hoy.AddDays(-6); // Hoy incluido = 7 días

            // 1. KPIs
            vm.IngresosTotales = await _db.Venta
                .SumAsync(v => (decimal?)v.Total) ?? 0m;

            vm.TotalVentas = await _db.Venta.CountAsync();
            vm.TotalClientes = await _db.Clientes.CountAsync();
            
            // Productos con stock <= 10
            vm.ProductosStockBajo = await _db.Productos
                .Include(p => p.IdCategoriaNavigation)
                .Where(p => p.Stock <= 10)
                .OrderBy(p => p.Stock)
                .Take(5)
                .ToListAsync();
            
            vm.AlertasStock = await _db.Productos.CountAsync(p => p.Stock <= 10);

            // 2. Gráfico de Área (Evolución últimos 7 días)
            var ventasUltimos7Dias = await _db.Venta
                .Where(v => v.Fecha >= hace7Dias)
                .GroupBy(v => v.Fecha.Date)
                .Select(g => new { Fecha = g.Key, Total = g.Sum(v => v.Total) })
                .ToListAsync();

            for (int i = 0; i < 7; i++)
            {
                var fechaEval = hace7Dias.AddDays(i);
                vm.FechasIngresos.Add(fechaEval.ToString("dd MMM", new CultureInfo("es-ES")));
                var ventaDia = ventasUltimos7Dias.FirstOrDefault(v => v.Fecha == fechaEval);
                vm.ValoresIngresos.Add(ventaDia != null ? ventaDia.Total : 0m);
            }

            // 3. Gráfico de Dona (Ventas por Categoría histórico)
            var ventasPorCategoria = await _db.DetalleVenta
                .Include(dv => dv.IdProductoNavigation)
                .ThenInclude(p => p.IdCategoriaNavigation)
                .GroupBy(dv => dv.IdProductoNavigation.IdCategoriaNavigation.Nombre)
                .Select(g => new { 
                    Categoria = g.Key, 
                    Ingresos = g.Sum(dv => dv.SubTotal) 
                })
                .OrderByDescending(x => x.Ingresos)
                .Take(5)
                .ToListAsync();

            foreach (var item in ventasPorCategoria)
            {
                vm.NombresCategorias.Add(item.Categoria);
                vm.ValoresCategorias.Add(item.Ingresos);
            }

            // 4. Gráfico de Barras Horizontales (Mejores Clientes)
            var mejoresClientes = await _db.Venta
                .Include(v => v.IdClienteNavigation)
                .Where(v => v.IdCliente != null)
                .GroupBy(v => v.IdClienteNavigation.Nombre)
                .Select(g => new {
                    Cliente = g.Key,
                    TotalComprado = g.Sum(v => v.Total)
                })
                .OrderByDescending(x => x.TotalComprado)
                .Take(5)
                .ToListAsync();
            
            foreach (var item in mejoresClientes)
            {
                vm.NombresMejoresClientes.Add(item.Cliente);
                vm.ValoresMejoresClientes.Add((decimal)item.TotalComprado);
            }

            // 4. Top 5 Productos Más Vendidos
            vm.TopProductos = await _db.DetalleVenta
                .Include(dv => dv.IdProductoNavigation)
                .GroupBy(dv => new { dv.IdProductoNavigation.Id, dv.IdProductoNavigation.Nombre })
                .Select(g => new ProductoTopVentas
                {
                    Nombre = g.Key.Nombre,
                    CantidadVendida = g.Sum(dv => dv.Cantidad),
                    IngresosGenerados = g.Sum(dv => dv.SubTotal)
                })
                .OrderByDescending(p => p.CantidadVendida)
                .Take(5)
                .ToListAsync();

            // 5. Capital Invertido por Categoría (Gráfico Polar)
            var capitalPorCategoria = await _db.Productos
                .Include(p => p.IdCategoriaNavigation)
                .GroupBy(p => p.IdCategoriaNavigation.Nombre)
                .Select(g => new {
                    Categoria = g.Key,
                    Capital = g.Sum(p => p.Stock * p.Precio)
                })
                .OrderByDescending(x => x.Capital)
                .ToListAsync();

            foreach (var item in capitalPorCategoria)
            {
                vm.NombresCapital.Add(item.Categoria);
                vm.ValoresCapital.Add(item.Capital);
            }

            // 6. Ticket Promedio (Últimos 7 días)
            var ventasParaTicket = await _db.Venta
                .Where(v => v.Fecha >= hace7Dias && v.Fecha <= hoy.AddDays(1).AddTicks(-1))
                .GroupBy(v => v.Fecha.Date)
                .Select(g => new {
                    Fecha = g.Key,
                    TotalVentas = g.Count(),
                    Ingresos = g.Sum(v => v.Total)
                })
                .ToListAsync();

            for (int i = 0; i < 7; i++)
            {
                var fechaEval = hace7Dias.AddDays(i);
                vm.FechasTicket.Add(fechaEval.ToString("dd MMM", new CultureInfo("es-ES")));
                
                var ventaDia = ventasParaTicket.FirstOrDefault(v => v.Fecha == fechaEval);
                if (ventaDia != null && ventaDia.TotalVentas > 0)
                {
                    vm.ValoresTicket.Add(ventaDia.Ingresos / ventaDia.TotalVentas);
                }
                else
                {
                    vm.ValoresTicket.Add(0m);
                }
            }

            return View(vm);
        }
    }
}
