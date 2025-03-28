using OceanOdyssey.Application.DTOs;
using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.Services.Interfaces
{
    public interface IServiceFechaCrucero
    {
        Task<ICollection<FechaCruceroDTO>> ListAsync();
        Task<FechaCruceroDTO> FindByIdAsync(int id);
        Task<ICollection<FechaCruceroDTO>> FechaXCrucero(int idCrucero);

        Task<ICollection<PrecioHabitacionDTO>> PreciosHabitacionesPorFecha(int idFechaCrucero);
    }
}
