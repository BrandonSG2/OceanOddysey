using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.DTOs
{
    public record ItinerarioDTO
    {
        public int Id { get; set; }

        public int? Idcrucero { get; set; }

        public int? Idpuerto { get; set; }

        public string? Descripcion { get; set; }

        public int Dia { get; set; }

        public virtual CruceroDTO? IdcruceroNavigation { get; set; }

        public virtual PuertoDTO? IdpuertoNavigation { get; set; }
    }
}
