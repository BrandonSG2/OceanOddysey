using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.DTOs
{
    public record UsuarioDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Clave { get; set; } = null!;

        public string? Rol { get; set; }

        public string? Telefono { get; set; }

        public string Email { get; set; } = null!;

        public DateOnly? Nacimiento { get; set; }

        public int Idpais { get; set; }

        public virtual PaisDTO IdpaisNavigation { get; set; } = null!;

        public virtual List<ResumenReservacionDTO> ResumenReservacion { get; set; } = null!;
    }
}
