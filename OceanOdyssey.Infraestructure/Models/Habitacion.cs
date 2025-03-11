using System;
using System.Collections.Generic;

namespace OceanOdyssey.Infraestructure.Models;

public partial class Habitacion
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Detalles { get; set; }

    public int CapacidadMaxima { get; set; }

    public int CapacidadMinima { get; set; }

    public int? Tamanno { get; set; }

    public virtual ICollection<BarcoHabitacion> BarcoHabitacion { get; set; } = new List<BarcoHabitacion>();

    public virtual ICollection<Pasajero> Pasajero { get; set; } = new List<Pasajero>();

    public virtual ICollection<PrecioHabitacion> PrecioHabitacion { get; set; } = new List<PrecioHabitacion>();

    public virtual ICollection<ReservaHabitacion> ReservaHabitacion { get; set; } = new List<ReservaHabitacion>();
}
