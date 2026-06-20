using System;
using System.Collections.Generic;

namespace AppCompleta.Models;

public partial class DetalleVenta
{
    public int Id { get; set; }

    public string HashId { get; set; } = null!;

    public int IdVenta { get; set; }

    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal SubTotal { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual Venta IdVentaNavigation { get; set; } = null!;
}
