using OceanOdyssey.Application.DTOs;
using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.Services.Interfaces
{
    public interface IServiceCrucero
    {
        Task<ICollection<CruceroDTO>> ListAsync();
        Task<CruceroDTO> FindByIdAsync(int id);
        Task<int> AddAsync(CruceroDTO dto);
        Task<CruceroDTO> FindByIdAsyncCrucero(int id);

    }
}
