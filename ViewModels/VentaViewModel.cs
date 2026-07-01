using AppCompleta.Models;

namespace AppCompleta.ViewModels
{
    public class VentaViewModel
    {
        public Venta venta { get; set; } = null!;
        public IEnumerable<Cliente>? clientes { get; set; }
    }
}
