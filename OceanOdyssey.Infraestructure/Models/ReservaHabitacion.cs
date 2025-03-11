using System;
using System.Collections.Generic;

namespace OceanOdyssey.Infraestructure.Models;

public partial class ReservaHabitacion
{
    public int Id { get; set; }

    public int? IdresumenReserva { get; set; }

    public int? Idhabitacion { get; set; }

    public int Idpasajero { get; set; }

    public virtual Habitacion? IdhabitacionNavigation { get; set; }

    public virtual Pasajero IdpasajeroNavigation { get; set; } = null!;

    public virtual ResumenReservacion? IdresumenReservaNavigation { get; set; }
}
