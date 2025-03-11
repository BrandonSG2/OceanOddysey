using System;
using System.Collections.Generic;

namespace OceanOdyssey.Infraestructure.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Clave { get; set; } = null!;

    public string? Rol { get; set; }

    public string? Telefono { get; set; }

    public string Email { get; set; } = null!;

    public DateOnly? Nacimiento { get; set; }

    public int Idpais { get; set; }

    public virtual Pais IdpaisNavigation { get; set; } = null!;

    public virtual ICollection<ResumenReservacion> ResumenReservacion { get; set; } = new List<ResumenReservacion>();
}
