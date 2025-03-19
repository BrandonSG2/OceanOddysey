
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



        [Display(Name = "Nombre")]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "El nombre no puede contener números.")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Nombre { get; set; } = null!;

        public byte[]? Imagen { get; set; }


        [Display(Name = "Duracion")]
        [Range(0, 9500, ErrorMessage = "El valor mínimo es {0}")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
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
