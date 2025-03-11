using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.DTOs
{
    public record ReservaComplementoDTO
    {
        public int Id { get; set; }

        public int? IdresumenReserva { get; set; }

        public int? Idcomplemento { get; set; }
        public int Cantidad { get; set; }

        public virtual ComplementoDTO? IdcomplementoNavigation { get; set; }

        public virtual ResumenReservacionDTO? IdresumenReservaNavigation { get; set; }
    }
}
