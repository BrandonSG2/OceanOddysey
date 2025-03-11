using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.DTOs
{
    public record CruceroDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public byte[]? Imagen { get; set; }

        public int Duracion { get; set; }

        [Display(Name = "Barco")]
        public int Idbarco { get; set; }

        public bool Disponible { get; set; }

        public decimal Costo { get; set; }

        public virtual List<FechaCruceroDTO> FechaCrucero { get; set; } = null!;

        public virtual BarcoDTO IdbarcoNavigation { get; set; } = null!;

        public virtual List<ItinerarioDTO> Itinerario { get; set; } = null!;

        public virtual List<ResumenReservacionDTO> ResumenReservacion { get; set; } = null!;
    }
}
