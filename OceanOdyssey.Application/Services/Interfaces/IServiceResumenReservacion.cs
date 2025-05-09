﻿using OceanOdyssey.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.Services.Interfaces
{
    public interface IServiceResumenReservacion
    {
        Task<ICollection<ResumenReservacionDTO>> ListAsync();
        Task<ResumenReservacionDTO> FindByIdAsync(int id);
        // Task<ResumenReservacionDTO> FindByIdAsyncCrucero(int id);
        Task<ICollection<ResumenReservacionDTO>> buscarXCruceroYfecha(int IDFechaCrucero);

        Task<int> AddAsync(ResumenReservacionDTO dto);

        Task<string> GenerarPdfResumenReservacionAsync(int id);


    }
}
