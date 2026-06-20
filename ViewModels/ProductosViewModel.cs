using AppCompleta.Models;

namespace AppCompleta.ViewModels
{
    public class ProductosViewModel
    {
        public Producto? Productos { get; set; } = null;
        public IEnumerable<Categoria>? Categorias { get; set; } = null!;
    }
}
