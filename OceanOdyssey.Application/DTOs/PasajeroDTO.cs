using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.DTOs
{
    public record PasajeroDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Telefono { get; set; }

        public DateOnly FechaNacimiento { get; set; }

        public int Idhabitacion { get; set; }

        public string Correo { get; set; } = null!;

        public string Direccion { get; set; } = null!;

        public string Sexo { get; set; } = null!;

        public virtual HabitacionDTO IdhabitacionNavigation { get; set; } = null!;

        public virtual List<ReservaHabitacionDTO> ReservaHabitacion { get; set; } = null!;
    }
}
