using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.DTOs
{
    public record ComplementoDTO
    {
        public int Id { get; set; }
        [Display(Name = "Nombre")]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "El nombre no puede contener números.")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Nombre { get; set; } = null!;
        [Display(Name = "Detalle")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string? Detalle { get; set; }
        [Required(ErrorMessage = "El precio es obligatorio.")]
        
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? Precio { get; set; }
        [Required(ErrorMessage = "Debe seleccionar a qué aplica el complemento")]
        [Display(Name = "Aplicado a")]
        public string? Aplicado { get; set; }
        public virtual List<ReservaComplementoDTO> ReservaComplemento { get; set; } = null!;
    }
}
