using OceanOdyssey.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.Services.Interfaces
{
    public interface IServicePasajero
    {
        Task<ICollection<PasajeroDTO>> ListAsync();
        Task<PasajeroDTO> FindByIdAsync(int id);

        Task<int> AddAsync(PasajeroDTO dto);
    }
}
