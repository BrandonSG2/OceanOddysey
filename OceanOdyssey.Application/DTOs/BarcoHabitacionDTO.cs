using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.DTOs
{
    public record BarcoHabitacionDTO
    {
        public int Id { get; set; }

        public int? Idbarco { get; set; }

        public int? Idhabitacion { get; set; }

        public int Cantidad { get; set; }

        public virtual BarcoDTO? IdbarcoNavigation { get; set; }

        public virtual HabitacionDTO? IdhabitacionNavigation { get; set; }
    }
}
