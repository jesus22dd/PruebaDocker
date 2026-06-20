using System;
using System.Collections.Generic;

namespace AppCompleta.Models;

public partial class Cliente
{
    public int Id { get; set; }

    public string HashId { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
