using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.DTOs
{
    public record FechaCruceroDTO
    {
        public int Id { get; set; }

        public int Idcrucero { get; set; }

        public DateOnly FechaInicio { get; set; }

        public virtual CruceroDTO IdcruceroNavigation { get; set; } = null!;
        public virtual List<PrecioHabitacionDTO> PrecioHabitacion { get; set; } = new List<PrecioHabitacionDTO>();

        public virtual List<ResumenReservacionDTO> ResumenReservacion { get; set; } = null!;
    }
}
