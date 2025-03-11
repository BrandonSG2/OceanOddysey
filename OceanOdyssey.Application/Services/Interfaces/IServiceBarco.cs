using OceanOdyssey.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.Services.Interfaces
{
    public interface IServiceBarco
    {
        Task<ICollection<BarcoDTO>> ListAsync();
        Task<BarcoDTO> FindByIdAsync(int id);
        Task<int> GetTotalHabitaciones(int idBarco);
    }
}
