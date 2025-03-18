using OceanOdyssey.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace OceanOdyssey.Web.ViewModels
{
    public class ViewModelInputHabitacion
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "El nombre no puede contener números.")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Nombre { get; set; } = null!;
        [Display(Name = "Detalles")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string? Detalles { get; set; }

        [Display(Name = "Capacidad Maxima")]
        [Range(1, int.MaxValue, ErrorMessage = "La capacidad máxima debe ser mayor a 0.")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int CapacidadMaxima { get; set; }

        [Display(Name = "Capacidad Minima")]
        [Range(1, int.MaxValue, ErrorMessage = "La capacidad mínima debe ser mayor a 0.")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int CapacidadMinima { get; set; }

        [Display(Name = "Tamaño")]
        [Range(0, 600, ErrorMessage = "El valor mínimo es {0}")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int? Tamanno { get; set; }

        public virtual List<BarcoHabitacionDTO> BarcoHabitacion { get; set; } = null!;

        public virtual List<PasajeroDTO> Pasajero { get; set; } = null!;

        public virtual List<PrecioHabitacionDTO> PrecioHabitacion { get; set; } = null!;

        public virtual List<ReservaHabitacionDTO> ReservaHabitacion { get; set; } = null!;

        [Display(Name = "Cantidad")]
        [Range(0, 600, ErrorMessage = "El valor mínimo es {0}")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int? Cantidad { get; set; }

    }
}
