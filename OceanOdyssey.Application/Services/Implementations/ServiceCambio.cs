using OceanOdyssey.Application.DTOs;
using OceanOdyssey.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.Services.Implementations
{
    public class ServiceCambio : IServiceCambio
    {
        public async Task<CambioDTO> ListAsync()
        {
            HttpClient httpClient = new HttpClient();
            string url = "https://tipodecambio.paginasweb.cr/api";

            CambioDTO tipoCambio = await httpClient.GetFromJsonAsync<CambioDTO>(url)
                ?? throw new InvalidOperationException("No se pudo obtener el tipo de cambio.");

            return tipoCambio;
        }
    }
}
