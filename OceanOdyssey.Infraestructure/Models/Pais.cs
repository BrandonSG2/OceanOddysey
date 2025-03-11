using System;
using System.Collections.Generic;

namespace OceanOdyssey.Infraestructure.Models;

public partial class Pais
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Puerto> Puerto { get; set; } = new List<Puerto>();

    public virtual ICollection<Usuario> Usuario { get; set; } = new List<Usuario>();
}
