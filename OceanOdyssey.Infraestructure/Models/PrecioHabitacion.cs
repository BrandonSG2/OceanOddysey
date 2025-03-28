using System;
using System.Collections.Generic;

namespace OceanOdyssey.Infraestructure.Models;

public partial class PrecioHabitacion
{
    public int Id { get; set; }

    public int Idhabitacion { get; set; }

    public decimal Costo { get; set; }

    public int? IdFechaCrucero { get; set; }

    public virtual FechaCrucero? IdFechaCruceroNavigation { get; set; }

    public virtual Habitacion IdhabitacionNavigation { get; set; } = null!;

}
