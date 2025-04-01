using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.DTOs
{
    public record PrecioHabitacionDTO
    {
        public int Id { get; set; }

        public int Idhabitacion { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal Costo { get; set; }
        public virtual FechaCruceroDTO? IdFechaCruceroNavigation { get; set; }

        public virtual HabitacionDTO IdhabitacionNavigation { get; set; } = null!;


    }
}
