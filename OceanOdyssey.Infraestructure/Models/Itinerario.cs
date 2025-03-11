using System;
using System.Collections.Generic;

namespace OceanOdyssey.Infraestructure.Models;

public partial class Itinerario
{
    public int Id { get; set; }

    public int? Idcrucero { get; set; }

    public int? Idpuerto { get; set; }

    public string? Descripcion { get; set; }

    public int Dia { get; set; }

    public virtual Crucero? IdcruceroNavigation { get; set; }

    public virtual Puerto? IdpuertoNavigation { get; set; }
}
