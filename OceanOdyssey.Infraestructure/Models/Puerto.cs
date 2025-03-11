using System;
using System.Collections.Generic;

namespace OceanOdyssey.Infraestructure.Models;

public partial class Puerto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int Idpais { get; set; }

    public virtual Pais IdpaisNavigation { get; set; } = null!;

    public virtual ICollection<Itinerario> Itinerario { get; set; } = new List<Itinerario>();
}
