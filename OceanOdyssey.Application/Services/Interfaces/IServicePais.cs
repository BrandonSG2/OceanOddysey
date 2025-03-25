using OceanOdyssey.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.Services.Interfaces
{
    public interface IServicePais
    {
        Task<ICollection<PaisDTO>> ListAsync();
        Task<PaisDTO> FindByIdAsync(int id);
    }
}
