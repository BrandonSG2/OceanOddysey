using System;
using System.Collections.Generic;

namespace OceanOdyssey.Infraestructure.Models;

public partial class FechaCrucero
{
    public int Id { get; set; }

    public int Idcrucero { get; set; }

    public DateOnly FechaInicio { get; set; }

    public virtual Crucero IdcruceroNavigation { get; set; } = null!;

    public virtual ICollection<PrecioHabitacion> PrecioHabitacion { get; set; } = new List<PrecioHabitacion>();

    public virtual ICollection<ResumenReservacion> ResumenReservacion { get; set; } = new List<ResumenReservacion>();
}
