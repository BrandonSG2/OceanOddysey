using System;
using System.Collections.Generic;

namespace OceanOdyssey.Infraestructure.Models;

public partial class Pasajero
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Telefono { get; set; }

    public DateOnly FechaNacimiento { get; set; }

    public int Idhabitacion { get; set; }

    public string Correo { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Sexo { get; set; } = null!;

    public virtual Habitacion IdhabitacionNavigation { get; set; } = null!;

    public virtual ICollection<ReservaHabitacion> ReservaHabitacion { get; set; } = new List<ReservaHabitacion>();
}
