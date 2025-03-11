using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.DTOs
{
    public record HabitacionDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Detalles { get; set; }

        public int CapacidadMaxima { get; set; }

        public int CapacidadMinima { get; set; }

        public int? Tamanno { get; set; }

        public virtual List<BarcoHabitacionDTO> BarcoHabitacion { get; set; } = null!;

        public virtual List<PasajeroDTO> Pasajero { get; set; } = null!;

        public virtual List<PrecioHabitacionDTO> PrecioHabitacion { get; set; } = null!;

        public virtual List<ReservaHabitacionDTO> ReservaHabitacion { get; set; } = null!;
    }
}
