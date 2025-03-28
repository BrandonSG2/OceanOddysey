using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.DTOs
{
    public record UsuarioDTO
    {
        public int Id { get; set; }


        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Nombre { get; set; } = null!;



        [Display(Name = "Contraseña")]
        [MinLength(6, ErrorMessage = "La {0} debe tener al menos 6 caracteres")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Clave { get; set; } = null!;


        [Display(Name = "Rol")]
        public string? Rol { get; set; }


        [Display(Name = "Telefono")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El {0} solo debe contener números.")]
        public string? Telefono { get; set; }


        [EmailAddress(ErrorMessage = "El formato del {0} no es válido")]
        [Display(Name = "Correo")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = "{0} es un dato requerido")]
        [Display(Name = "Fecha de nacimiento")]
        public DateOnly? Nacimiento { get; set; }


        [Display(Name = "Codigo de Pais")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int Idpais { get; set; }

        public virtual PaisDTO? IdpaisNavigation { get; set; }

        public virtual List<ResumenReservacionDTO> ResumenReservacion { get; set; } = new List<ResumenReservacionDTO>();
    }
}
