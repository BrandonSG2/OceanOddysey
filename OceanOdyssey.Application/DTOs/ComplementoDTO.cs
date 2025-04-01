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

        public string Nombre { get; set; } = null!;

        public string? Detalle { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? Precio { get; set; }

        public virtual List<ReservaComplementoDTO> ReservaComplemento { get; set; } = null!;
    }
}
