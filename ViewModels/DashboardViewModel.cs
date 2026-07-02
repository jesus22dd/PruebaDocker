using AppCompleta.Models;

namespace AppCompleta.ViewModels
{
    public class DashboardViewModel
    {
        // KPIs
        public decimal IngresosTotales { get; set; }
        public int TotalVentas { get; set; }
        public int TotalClientes { get; set; }
        public int AlertasStock { get; set; }

        // Gráfico de Área (Evolución de Ingresos últimos 7 días)
        public List<string> FechasIngresos { get; set; } = new List<string>();
        public List<decimal> ValoresIngresos { get; set; } = new List<decimal>();

        // Gráfico de Dona (Ventas por Categoría)
        public List<string> NombresCategorias { get; set; } = new List<string>();
        public List<decimal> ValoresCategorias { get; set; } = new List<decimal>();

        // Gráfico de Barras (Mejores Clientes)
        public List<string> NombresMejoresClientes { get; set; } = new List<string>();
        public List<decimal> ValoresMejoresClientes { get; set; } = new List<decimal>();

        // Gráfico Polar (Capital por Categoría)
        public List<string> NombresCapital { get; set; } = new List<string>();
        public List<decimal> ValoresCapital { get; set; } = new List<decimal>();

        // Gráfico Tendencia (Ticket Promedio 7 días)
        public List<string> FechasTicket { get; set; } = new List<string>();
        public List<decimal> ValoresTicket { get; set; } = new List<decimal>();

        // Tablas
        public List<ProductoTopVentas> TopProductos { get; set; } = new List<ProductoTopVentas>();
        public List<Producto> ProductosStockBajo { get; set; } = new List<Producto>();
    }

    public class ProductoTopVentas
    {
        public string Nombre { get; set; } = string.Empty;
        public int CantidadVendida { get; set; }
        public decimal IngresosGenerados { get; set; }
    }
}
