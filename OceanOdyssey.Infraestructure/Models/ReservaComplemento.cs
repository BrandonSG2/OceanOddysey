using System;
using System.Collections.Generic;

namespace OceanOdyssey.Infraestructure.Models;

public partial class ReservaComplemento
{
    public int Id { get; set; }

    public int? IdresumenReserva { get; set; }

    public int? Idcomplemento { get; set; }

    public int Cantidad { get; set; }

    public virtual Complemento? IdcomplementoNavigation { get; set; }

    public virtual ResumenReservacion? IdresumenReservaNavigation { get; set; }
}
