using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.DTOs
{
    public class CambioDTO
    {
        public decimal venta { get; set; }
        public decimal compra { get; set; }
        public string cambio_dolar { get; set; }
    }
}
