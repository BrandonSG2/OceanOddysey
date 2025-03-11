using System;
using System.Collections.Generic;

namespace OceanOdyssey.Infraestructure.Models;

public partial class ResumenReservacion
{
    public int Id { get; set; }

    public int Idusuario { get; set; }

    public int Idcrucero { get; set; }

    public DateOnly FechaReservacion { get; set; }

    public int CantidadHabitaciones { get; set; }

    public decimal PrecioTotal { get; set; }

    public decimal Impuestos { get; set; }

    public decimal TotalFinal { get; set; }

    public string Estado { get; set; } = null!;

    public int FechaCrucero { get; set; }

    public DateOnly? FechaPago { get; set; }

    public decimal TotalHabitaciones { get; set; }

    public virtual FechaCrucero FechaCruceroNavigation { get; set; } = null!;

    public virtual Crucero IdcruceroNavigation { get; set; } = null!;

    public virtual Usuario IdusuarioNavigation { get; set; } = null!;

    public virtual ICollection<ReservaComplemento> ReservaComplemento { get; set; } = new List<ReservaComplemento>();

    public virtual ICollection<ReservaHabitacion> ReservaHabitacion { get; set; } = new List<ReservaHabitacion>();
}
