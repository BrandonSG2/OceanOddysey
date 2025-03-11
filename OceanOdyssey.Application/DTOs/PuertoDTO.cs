using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.DTOs
{
    public record PuertoDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;
        
        public int Idpais { get; set; }

        public virtual PaisDTO IdpaisNavigation { get; set; } = null!;

        public virtual List<ItinerarioDTO> Itinerario { get; set; } = null!;
    }
}
