using System;
using System.Collections.Generic;

namespace AppCompleta.Models;

public partial class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Detalle { get; set; }

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public int IdCategoria { get; set; }

    public string HashId { get; set; } = null!;

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual Categoria IdCategoriaNavigation { get; set; } = null!;
}
