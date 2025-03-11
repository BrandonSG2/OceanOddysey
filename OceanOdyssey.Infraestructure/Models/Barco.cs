using System;
using System.Collections.Generic;

namespace OceanOdyssey.Infraestructure.Models;

public partial class Barco
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int Capacidad { get; set; }

    public string? Descripcion { get; set; }

    public int? TotalHabitaciones { get; set; }

    public virtual ICollection<BarcoHabitacion> BarcoHabitacion { get; set; } = new List<BarcoHabitacion>();

    public virtual ICollection<Crucero> Crucero { get; set; } = new List<Crucero>();
}
