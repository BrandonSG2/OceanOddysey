using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.DTOs
{
    public record BarcoDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public int Capacidad { get; set; }

        public string? Descripcion { get; set; }

        public int? TotalHabitaciones { get; set; }

        public virtual List<BarcoHabitacionDTO> BarcoHabitacion { get; set; }  = null!;
        public virtual List<CruceroDTO> Crucero { get; set; }   = null!;




    }
}
