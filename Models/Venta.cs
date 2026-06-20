using System;
using System.Collections.Generic;

namespace AppCompleta.Models;

public partial class Venta
{
    public int Id { get; set; }

    public string HashId { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public decimal Total { get; set; }

    public int? IdCliente { get; set; }

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual Cliente? IdClienteNavigation { get; set; }
}
