using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.DTOs
{
    public record ResumenReservacionDTO
    {
        [Display(Name = "Identificador Reserva")]
        public int Id { get; set; }
        [Display(Name = "Identificador Usuario")]
        public int Idusuario { get; set; }
        [Display(Name = "Identificador Crucero")]
        public int Idcrucero { get; set; }
        [Display(Name = "Fecha Reservacion")]
        public DateOnly FechaReservacion { get; set; }
        [Display(Name = "Habitaciones")]
        public int CantidadHabitaciones { get; set; }
        [Display(Name = "Subtotal")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal PrecioTotal { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal Impuestos { get; set; }
        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal TotalFinal { get; set; }

        public string Estado { get; set; } = null!;
        
        public int FechaCrucero { get; set; }

        public DateOnly? FechaPago { get; set; }
        [Display(Name = "Total a pagar por habitaciones")]
        public decimal TotalHabitaciones { get; set; }

        public virtual FechaCruceroDTO FechaCruceroNavigation { get; set; } = null!;

        public virtual CruceroDTO IdcruceroNavigation { get; set; } = null!;

        public virtual UsuarioDTO IdusuarioNavigation { get; set; } = null!;

        public virtual List<ReservaComplementoDTO> ReservaComplemento { get; set; } = null!;

        public virtual List<ReservaHabitacionDTO> ReservaHabitacion { get; set; } = null!;
    }
}
