using System;
using System.Collections.Generic;

namespace OceanOdyssey.Infraestructure.Models;

public partial class BarcoHabitacion
{
    public int Id { get; set; }

    public int? Idbarco { get; set; }

    public int? Idhabitacion { get; set; }

    public int Cantidad { get; set; }

    public virtual Barco? IdbarcoNavigation { get; set; }

    public virtual Habitacion? IdhabitacionNavigation { get; set; }
}
