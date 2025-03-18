using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.DTOs
{
    public record BarcoDTO
    {
        public int id { get; set; }

        [Display(Name = "Nombre")]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "El nombre no puede contener números.")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Nombre { get; set; } = null!;

        [Display(Name = "Capacidad")]
        [Range(0,9500, ErrorMessage = "El valor mínimo es {0}")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int Capacidad { get; set; }

        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string? Descripcion { get; set; }

        [Display(Name = "TotalHabitaciones")]
        [Range(0, 8000, ErrorMessage = "El valor mínimo es {0}")]
       
        public int? TotalHabitaciones { get; set; }
        [Display(Name = "Habitaciones")]
        
        public virtual List<BarcoHabitacionDTO> BarcoHabitacion { get; set; } = new();
        public virtual List<CruceroDTO> Crucero { get; set; } = new();




    }
}
