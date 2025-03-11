using System;
using System.Collections.Generic;

namespace OceanOdyssey.Infraestructure.Models;

public partial class Crucero
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public byte[]? Imagen { get; set; }

    public int Duracion { get; set; }

    public int Idbarco { get; set; }

    public bool Disponible { get; set; }

    public decimal Costo { get; set; }

    public virtual ICollection<FechaCrucero> FechaCrucero { get; set; } = new List<FechaCrucero>();

    public virtual Barco IdbarcoNavigation { get; set; } = null!;

    public virtual ICollection<Itinerario> Itinerario { get; set; } = new List<Itinerario>();

    public virtual ICollection<ResumenReservacion> ResumenReservacion { get; set; } = new List<ResumenReservacion>();
}
