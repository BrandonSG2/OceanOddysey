using System;
using System.Collections.Generic;

namespace OceanOdyssey.Infraestructure.Models;

public partial class Complemento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Detalle { get; set; }

    public decimal? Precio { get; set; }

    public virtual ICollection<ReservaComplemento> ReservaComplemento { get; set; } = new List<ReservaComplemento>();
}
