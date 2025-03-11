using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.DTOs
{
    public record ReservaHabitacionDTO
    {
        public int Id { get; set; }

        public int? IdresumenReserva { get; set; }

        public int? Idhabitacion { get; set; }

        public int Idpasajero { get; set; }

        public virtual HabitacionDTO? IdhabitacionNavigation { get; set; }

        public virtual PasajeroDTO IdpasajeroNavigation { get; set; } = null!;

        public virtual ResumenReservacionDTO? IdresumenReservaNavigation { get; set; }
    }
}
